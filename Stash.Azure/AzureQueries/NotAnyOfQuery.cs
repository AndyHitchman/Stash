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

    public class NotAnyOfQuery<TKey> : IAzureIndexQuery, INotAnyOfQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly ManagedIndex managedIndex;

        public NotAnyOfQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, IEnumerable<TKey> set)
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
            var query = managedIndex.ReverseIndex(serviceContext);

            Func<InternalId, InternalId> nextInternalId =
                cid =>
                    {
                        var next =
                            (from ri in query
                             where ri.PartitionKey.CompareTo(cid.Value.ToString()) > 0
                             select ri)
                                .FirstOrDefault();
                        return next != null ? managedIndex.ConvertToInternalId(next.PartitionKey) : null;
                    };

            var matchingAny = new AnyOfQuery<TKey>(managedIndex, Indexer, Set).Execute(serviceContext).Materialize();
            
            var currentInternalId = new InternalId(Guid.Empty);
            while ((currentInternalId = nextInternalId(currentInternalId)) != null)
                if(!matchingAny.Contains(currentInternalId))
                    yield return currentInternalId;
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(TableServiceContext serviceContext, IEnumerable<InternalId> joinConstraint)
        {
            if(joinConstraint.Count() > EstimatedQueryCost(serviceContext))
                return Execute(serviceContext);

            return
                joinConstraint.Except(
                    joinConstraint.Aggregate(
                        Enumerable.Empty<InternalId>(),
                        (matching, joinMatched) => matching.Union(
                            IndexMatching
                                .GetReverseMatching<TKey>(managedIndex, serviceContext, joinMatched)
                                .Where(key => Set.Contains(key))
                                .Select(_ => joinMatched))));
        }

        public IAnyOfQuery<TKey> GetComplementaryQuery()
        {
            return new AnyOfQuery<TKey>(managedIndex, Indexer, Set);
        }
    }
}