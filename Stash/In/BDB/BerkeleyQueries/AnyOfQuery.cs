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
            return Set.Aggregate(Enumerable.Empty<Guid>(), (current, key) => current.Union(IndexMatching.GetMatching(managedIndex, transaction, key)));
        }

        public IEnumerable<Guid> ExecuteInsideIntersect(ManagedIndex managedIndex, Transaction transaction, IEnumerable<Guid> joinConstraint)
        {
            return Execute(managedIndex, transaction);
        }

        public INotAnyOfQuery<TKey> GetComplementaryQuery()
        {
            return new NotAnyOfQuery<TKey>(Indexer, Set);
        }
    }
}