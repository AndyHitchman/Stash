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
    using System.Threading.Tasks;
    using Engine;
    using Microsoft.WindowsAzure.StorageClient;
    using Stash.Azure;
    using Stash.Configuration;
    using Stash.Engine;
    using Stash.Queries;

    public class OutsideQuery<TKey> : IAzureIndexQuery, IOutsideQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly ManagedIndex managedIndex;

        public OutsideQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey)
        {
            this.managedIndex = managedIndex;
            Indexer = indexer;
            LowerKey = lowerKey;
            UpperKey = upperKey;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public TKey LowerKey { get; private set; }
        public TKey UpperKey { get; private set; }

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
            var lower =
                (from fi in managedIndex.ForwardIndex(serviceContext)
                 where fi.PartitionKey.CompareTo(managedIndex.KeyAsString(LowerKey)) < 0
                 select fi)
                    .AsEnumerable();

            var upper =
                (from fi in managedIndex.ForwardIndex(serviceContext)
                 where fi.PartitionKey.CompareTo(managedIndex.KeyAsString(UpperKey)) > 0
                 select fi)
                    .AsEnumerable();

            return
                lower.Union(upper)
                    .AsParallel()
                    .Select(fi => managedIndex.ConvertToInternalId(fi.RowKey));
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(TableServiceContext serviceContext, IEnumerable<InternalId> joinConstraint)
        {
            if(joinConstraint.Count() > EstimatedQueryCost(serviceContext))
                return Execute(serviceContext);

            return
                joinConstraint.Aggregate(
                    Enumerable.Empty<InternalId>(),
                    (matching, joinMatched) => matching.Union(
                        IndexMatching
                            .GetReverseMatching<TKey>(managedIndex, serviceContext, joinMatched)
                            .Where(key => key.CompareTo(LowerKey) < 0 || key.CompareTo(UpperKey) > 0)
                            .Select(_ => joinMatched)));
        }

        public IBetweenQuery<TKey> GetComplementaryQuery()
        {
            return new BetweenQuery<TKey>(managedIndex, Indexer, LowerKey, UpperKey);
        }
    }
}