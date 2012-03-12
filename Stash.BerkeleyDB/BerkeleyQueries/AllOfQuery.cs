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

namespace Stash.BerkeleyDB.BerkeleyQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using global::BerkeleyDB;
    using Stash.Configuration;
    using Stash.Engine;
    using Stash.Queries;

    public class AllOfQuery<TKey> : IBerkeleyIndexQuery, IAllOfQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly ManagedIndex managedIndex;

        public AllOfQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, IEnumerable<TKey> set, QueryCostScale queryCostScaleHint)
            : this(managedIndex, indexer, set)
        {}

        public AllOfQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, IEnumerable<TKey> set)
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

        public double EstimatedQueryCost(Transaction transaction)
        {
            return (double)QueryCostScale * Set.Count();
        }

        public IEnumerable<InternalId> Execute(Transaction transaction)
        {
            //The seed of the aggregate is the matches for the first element of the set. The remaineder of the set
            //is passed as the comparison set.
            var matchingFirst = IndexMatching.GetMatching(managedIndex, transaction, Set.First());
            return execute(transaction, matchingFirst, Set.Skip(1));
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(Transaction transaction, IEnumerable<InternalId> joinConstraint)
        {
            //The seed of the aggregate is the join constraint. The full set is passed as the comparison set.
            return execute(transaction, joinConstraint, Set);
        }

        public INotAllOfQuery<TKey> GetComplementaryQuery()
        {
            return new NotAllOfQuery<TKey>(managedIndex, Indexer, Set);
        }

        private IEnumerable<InternalId> execute(Transaction transaction, IEnumerable<InternalId> joinMatching, IEnumerable<TKey> comparisonSubset)
        {
            //If the number of graphs in the constraint is less than the size of the set,
            //then it should be quicker to hit the reverse index for all graphs and eliminate,
            //rather than aggregrating the intersection.
            return joinMatching.Count() < Set.Count()
                       ? joinMatching.Where(
                           internalId =>
                               {
                                   var reverseMatching = IndexMatching.GetReverseMatching<TKey>(managedIndex, transaction, internalId);
                                   return Set.All(key => reverseMatching.Contains(key));
                               })
                       : comparisonSubset.Aggregate(joinMatching, (current, key) => current.Intersect(IndexMatching.GetMatching(managedIndex, transaction, key)));
        }
    }
}