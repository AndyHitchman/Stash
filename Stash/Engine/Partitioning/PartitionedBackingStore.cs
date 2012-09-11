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
    using Serializers;

    public class PartitionedBackingStore : IBackingStore
    {
        private readonly IBackingStore underlyingBackingStore;

        public PartitionedBackingStore(IPartition partition, IBackingStore underlyingBackingStore)
        {
            Partition = partition;
            this.underlyingBackingStore = underlyingBackingStore;
        }

        public IPartition Partition { get; private set; }

        public IQueryFactory QueryFactory
        {
            get { return underlyingBackingStore.QueryFactory; }
        }

        public void Close()
        {
            underlyingBackingStore.Close();
        }

        public int Count(IQuery query)
        {
            var count = 0;

            InTransactionDo(
                work => { count = work.Count(query); });

            return count;
        }

        public void EnsureIndex(IRegisteredIndexer registeredIndexer)
        {
            underlyingBackingStore.EnsureIndex(registeredIndexer);
        }

        public IProjectedIndex ProjectIndex<TKey>(string indexName, TKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            var storedGraphs = Enumerable.Empty<IStoredGraph>();

            InTransactionDo(
                work => { storedGraphs = work.Get(query); });

            return storedGraphs;
        }

        public IStoredGraph Get(InternalId internalId)
        {
            IStoredGraph storedGraph = null;

            InTransactionDo(
                work => { storedGraph = work.Get(internalId); });

            return storedGraph;
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            //Filter all storage work actions through the partitioned storage worker,
            //which is responsible for determining whether its partition should do
            //anything with the action.
            underlyingBackingStore.InTransactionDo(
                work =>
                    {
                        var partitionedWork = new PartitionedStorageWork(Partition, work);
                        storageWorkActions(partitionedWork);
                    });
        }

        public bool IsTypeSupportedInIndexes(Type proposedIndexType)
        {
            throw new NotImplementedException();
        }

        public ISerializer<TGraph> GetDefaultSerialiser<TGraph>(IRegisteredGraph<TGraph> registeredGraph)
        {
            throw new NotImplementedException();
        }

        public IStoredGraph CreateStoredGraph(Type graphType)
        {
            throw new NotImplementedException();
        }
    }
}