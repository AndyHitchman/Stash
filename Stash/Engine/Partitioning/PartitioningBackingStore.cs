#region License
// Copyright 2009, 2010 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Queries;

    /// <summary>
    ///   The partitioned backing store allows multiple backing stores to share resposibility for persisting data.
    ///   Each operation on the backing store is blindly dispatched to each patitioned backing store. These backing stores
    ///   can either execute the requested operation or attempt to determine whether they are responsible for any data
    ///   affected by the operation.
    /// </summary>
    public class PartitioningBackingStore : IBackingStore
    {
        private readonly IDictionary<IPartition, IBackingStore> partitionedBackingStores;

        public PartitioningBackingStore(IDictionary<IPartition, IBackingStore> partitionedBackingStores)
        {
            this.partitionedBackingStores = partitionedBackingStores;
            QueryFactory = new PartitioningQueryFactory(partitionedBackingStores.ToDictionary(_ => _.Key, _ => _.Value.QueryFactory));
        }

        public IQueryFactory QueryFactory { get; private set; }

        public void Close()
        {
            fireAtPartitions((partition, backingStore) => backingStore.Close());
        }

        public int Count(IQuery query)
        {
            var partitionedQuery = (IPartitionedQuery)query;

            return
                collectFromPartitions(
                    (partition, backingStore) => backingStore.Count(partitionedQuery.GetQueryForPartition(partition)),
                    0,
                    (accumlator, partial) => accumlator + partial);
        }


        public void EnsureIndex(IRegisteredIndexer registeredIndexer)
        {
            fireAtPartitions((partition, backingStore) => backingStore.EnsureIndex(registeredIndexer));
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            var partitionedQuery = (IPartitionedQuery)query;

            return
                collectFromPartitions(
                    (partition, backingStore) => backingStore.Get(partitionedQuery.GetQueryForPartition(partition)),
                    Enumerable.Empty<IStoredGraph>(),
                    (accumulator, partial) => accumulator.Union(partial));
        }

        public IStoredGraph Get(InternalId internalId)
        {
            var storedGraph =
                collectFromPartitions(
                    (partition, backingStore) => backingStore.Get(internalId),
                    null,
                    (accumulator, partial) => accumulator ?? partial);

            if(storedGraph == null)
                throw new GraphForKeyNotFoundException(internalId, null);

            return storedGraph;
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            fireAtPartitions((partition, backingStore) => backingStore.InTransactionDo(storageWorkActions));
        }

        private TResult collectFromPartitions<TResult>(Func<IPartition, IBackingStore, TResult> map, TResult seed, Func<TResult, TResult, TResult> reduce)
        {
            //Fire off async invokes of the map function, collecting the async results in an enumerable.
            //Then aggregate the results via the supplied reduce function.
            return
                partitionedBackingStores
                    .Aggregate(
                        Enumerable.Empty<IAsyncResult>(),
                        (dispatch, partitionStorePair) => dispatch.Concat(new[] {map.BeginInvoke(partitionStorePair.Key, partitionStorePair.Value, null, null)}))
                    .Aggregate(
                        seed,
                        (acc, asyncResult) => reduce(acc, map.EndInvoke(asyncResult)));
        }

        private void fireAtPartitions(Action<IPartition, IBackingStore> action)
        {
            //Fire off async invokes of the map function, collecting the async results in an enumerable.
            //Then aggregate the results via the supplied reduce function.
            foreach(
                var asyncResult 
                    in partitionedBackingStores
                        .Aggregate(
                            Enumerable.Empty<IAsyncResult>(),
                            (dispatch, partitionStorePair) => dispatch.Concat(new[] {action.BeginInvoke(partitionStorePair.Key, partitionStorePair.Value, null, null)})))
            {
                action.EndInvoke(asyncResult);
            }
        }
    }
}