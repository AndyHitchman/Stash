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
    using Engine;
    using global::BerkeleyDB;
    using Stash.Configuration;
    using Stash.Engine;
    using Stash.Queries;

    public class EqualToQuery<TKey> : IBerkeleyIndexQuery, IEqualToQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly ManagedIndex managedIndex;

        public EqualToQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, TKey key)
        {
            this.managedIndex = managedIndex;
            Indexer = indexer;
            Key = key;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public TKey Key { get; private set; }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.SingleGet; }
        }

        public double EstimatedQueryCost(Transaction transaction)
        {
            return (double)QueryCostScale;
        }

        public IEnumerable<InternalId> Execute(Transaction transaction)
        {
            try
            {
                return managedIndex.Index
                    .GetMultiple(new DatabaseEntry(managedIndex.KeyAsByteArray(Key)), (int)managedIndex.Index.Pagesize, transaction)
                    .Value
                    .Select(graphKey => graphKey.Data.AsInternalId());
            }
            catch(NotFoundException)
            {
                return Enumerable.Empty<InternalId>();
            }
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(Transaction transaction, IEnumerable<InternalId> joinConstraint)
        {
            //Can't do better than this.
            return Execute(transaction);
        }

        public INotEqualToQuery<TKey> GetComplementaryQuery()
        {
            return new NotEqualToQuery<TKey>(managedIndex, Indexer, Key);
        }
    }
}