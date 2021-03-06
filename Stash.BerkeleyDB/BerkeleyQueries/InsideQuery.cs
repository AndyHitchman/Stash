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

    public class InsideQuery<TKey> : IBerkeleyIndexQuery, IInsideQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private const int pageSizeBufferMultipler = 4;
        private readonly ManagedIndex managedIndex;

        public InsideQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey)
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
            get { return QueryCostScale.ClosedRangeScan; }
        }

        public double EstimatedQueryCost(Transaction transaction)
        {
            return (managedIndex.Index.KeyRange(new DatabaseEntry(managedIndex.KeyAsByteArray(UpperKey)), transaction).Less -
                    managedIndex.Index.KeyRange(new DatabaseEntry(managedIndex.KeyAsByteArray(LowerKey)), transaction).Less) *
                   managedIndex.Index.FastStats().nPages / pageSizeBufferMultipler * (double)QueryCostScale;
        }

        public IEnumerable<InternalId> Execute(Transaction transaction)
        {
            var cursor = managedIndex.Index.Cursor(new CursorConfig(), transaction);
            try
            {
                var comparer = managedIndex.Comparer;
                var keyAsBytes = managedIndex.KeyAsByteArray(LowerKey);
                var bufferSize = (int)managedIndex.Index.Pagesize * pageSizeBufferMultipler;

                //The exact value and then the next unique
                if(cursor.Move(new DatabaseEntry(keyAsBytes), true) | cursor.MoveNextUniqueMultipleKey(bufferSize))
                {
                    //There could be an exact equal match and then no further unique matches.
                    if(cursor.CurrentMultipleKey != null)
                        do
                        {
                            foreach(var internalId in
                                cursor.CurrentMultipleKey
                                    .Select(_ => new {key = _.Key.Data, value = _.Value.Data.AsInternalId()})
                                    .TakeWhile(pair => comparer.Compare(managedIndex.ByteArrayAsKey(pair.key), UpperKey) < 0)
                                    .Select(_ => _.value))
                            {
                                yield return internalId;
                            }
                        }
                        while(cursor.MoveNextMultipleKey(bufferSize) &&
                              comparer.Compare(managedIndex.ByteArrayAsKey(cursor.CurrentMultipleKey.First().Key.Data), UpperKey) < 0);
                }
            }
            finally
            {
                cursor.Close();
            }
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(Transaction transaction, IEnumerable<InternalId> joinConstraint)
        {
            if(joinConstraint.Count() > EstimatedQueryCost(transaction))
                return Execute(transaction);

            var comparer = managedIndex.Comparer;

            return
                joinConstraint.Aggregate(
                    Enumerable.Empty<InternalId>(),
                    (matching, joinMatched) => matching.Union(
                        IndexMatching
                            .GetReverseMatching<TKey>(managedIndex, transaction, joinMatched)
                            .Where(key => comparer.Compare(key, LowerKey) > 0 & comparer.Compare(key, UpperKey) < 0)
                            .Select(_ => joinMatched)));
        }
    }
}