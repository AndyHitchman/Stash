#region License

// Copyright 2009 Andrew Hitchman
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

namespace Stash.In.BDB.BerkeleyQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using Configuration;
    using Queries;

    public class AnyOfQuery<TKey> : IBerkeleyIndexQuery, IAnyOfQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public AnyOfQuery(IRegisteredIndexer indexer, IEnumerable<TKey> set)
        {
            Indexer = indexer;
            Set = set;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public IEnumerable<TKey> Set { get; private set; }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.MultiGet; }
        }

        public double EstimatedQueryCost(ManagedIndex managedIndex, Transaction transaction)
        {
            return (double)QueryCostScale * Set.Count();
        }

        public IEnumerable<Guid> Execute(ManagedIndex managedIndex, Transaction transaction)
        {
            return Set.Aggregate(Enumerable.Empty<Guid>(), (current, key) => current.Union(getMatching(managedIndex, key, transaction)));
        }

        public IEnumerable<Guid> ExecuteInsideIntersect(ManagedIndex managedIndex, Transaction transaction, IEnumerable<Guid> joinConstraint)
        {
            //TODO: Think of a better approach than simply throwing away the advantage of the other half of the intersect.
            return Execute(managedIndex, transaction);
        }

        private static IEnumerable<Guid> getMatching(ManagedIndex managedIndex, TKey key, Transaction transaction)
        {
            try
            {
                return managedIndex.Index.GetMultiple(new DatabaseEntry(managedIndex.KeyAsByteArray(key)), (int)managedIndex.Index.Pagesize, transaction)
                    .Value
                    .Select(graphKey => graphKey.Data.AsGuid());
            }
            catch(NotFoundException)
            {
                return Enumerable.Empty<Guid>();
            }
        }

        public INotAnyOfQuery<TKey> GetComplementaryQuery()
        {
            return new NotAnyOfQuery<TKey>(Indexer, Set);
        }
    }
}