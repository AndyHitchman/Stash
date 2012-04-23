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

namespace Stash.Azure.AzureQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.StorageClient;
    using Azure;
    using Configuration;
    using Engine;
    using Queries;

    public class NotAllOfQuery<TKey> : IAzureIndexQuery, INotAllOfQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private const int pageSizeBufferMultipler = 4;
        private readonly ManagedIndex managedIndex;

        public NotAllOfQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, IEnumerable<TKey> set)
        {
            this.managedIndex = managedIndex;
            Indexer = indexer;
            Set = set;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public IEnumerable<TKey> Set { get; private set; }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.FullScan; }
        }

        public double EstimatedQueryCost(TableServiceContext serviceContext)
        {
            return (double)QueryCostScale;
        }

        public IEnumerable<InternalId> Execute(TableServiceContext serviceContext)
        {
            var query = managedIndex.ForwardIndex(serviceContext).AsQueryable();
            foreach(var key in Set)
            {
                var closedKeyString = managedIndex.KeyAsString(key);
                query = query.Where(fi => fi.PartitionKey != closedKeyString);
            }
            return query.AsEnumerable().Select(fi => managedIndex.ConvertToInternalId(fi.RowKey));
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(TableServiceContext serviceContext, IEnumerable<InternalId> joinConstraint)
        {
            if(joinConstraint.Count() > EstimatedQueryCost(serviceContext))
                return Execute(serviceContext);

            return
                joinConstraint
                    .AsParallel()
                    .Aggregate(
                        Enumerable.Empty<InternalId>(),
                        (matching, joinMatched) => matching.Union(
                            IndexMatching
                                .GetReverseMatching<TKey>(managedIndex, serviceContext, joinMatched)
                                .Where(key => !Set.Contains(key))
                                .Select(_ => joinMatched)));
        }

        public IAllOfQuery<TKey> GetComplementaryQuery()
        {
            return new AllOfQuery<TKey>(managedIndex, Indexer, Set);
        }
    }
}