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

    public class AllOfQuery<TKey> : IBerkeleyQuery, IAllOfQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public AllOfQuery(IRegisteredIndexer indexer, IEnumerable<TKey> set)
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
            var matchingFirst = getMatching(managedIndex, Set.First(), transaction);
            //If the number of graphs in the seed is less than the size of the set,
            //then it should be quicker to hit the reverse index for all graphs and eliminate,
            //rather than aggregrating the intersection.
            return matchingFirst.Count() < Set.Count()
                       ? matchingFirst.Where(internalId =>
                                                 {
                                                     var reverseMatching = getReverseMatching(managedIndex, internalId, transaction);
                                                     return Set.All(key => reverseMatching.Contains(key));
                                                 })
                       : Set.Skip(1).Aggregate(matchingFirst, (current, key) => current.Intersect(getMatching(managedIndex, key, transaction)));
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

        private static IEnumerable<TKey> getReverseMatching(ManagedIndex managedIndex, Guid guid, Transaction transaction)
        {
            try
            {
                return managedIndex.ReverseIndex.GetMultiple(new DatabaseEntry(guid.AsByteArray()), (int)managedIndex.Index.Pagesize, transaction)
                    .Value
                    .Select(graphKey => (TKey)managedIndex.ByteArrayAsKey(graphKey.Data));
            }
            catch(NotFoundException)
            {
                return Enumerable.Empty<TKey>();
            }
        }
    }
}