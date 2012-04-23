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
    using Stash.Azure;
    using Microsoft.WindowsAzure.StorageClient;
    using Stash.Configuration;
    using Stash.Engine;
    using Stash.Queries;

    public class AnyOfQuery<TKey> : IAzureIndexQuery, IAnyOfQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly ManagedIndex managedIndex;

        public AnyOfQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, IEnumerable<TKey> set)
        {
            this.managedIndex = managedIndex;
            Indexer = indexer;
            Set = set;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public IEnumerable<TKey> Set { get; private set; }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.MultiGet; }
        }

        public double EstimatedQueryCost(TableServiceContext serviceContext)
        {
            return (double)QueryCostScale * Set.Count();
        }

        public IEnumerable<InternalId> Execute(TableServiceContext serviceContext)
        {
            return
                Set
                    .AsParallel()
                    .Aggregate(Enumerable.Empty<InternalId>(), (current, key) => current.Union(IndexMatching.GetMatching(managedIndex, serviceContext, key)));
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(TableServiceContext serviceContext, IEnumerable<InternalId> joinConstraint)
        {
            return Execute(serviceContext);
        }

        public INotAnyOfQuery<TKey> GetComplementaryQuery()
        {
            return new NotAnyOfQuery<TKey>(managedIndex, Indexer, Set);
        }
    }
}