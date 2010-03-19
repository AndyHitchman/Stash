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

namespace Stash.BackingStore.BDB.BerkeleyQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using Configuration;
    using Queries;

    public class LessThanEqualToQuery<TKey> : IBerkeleyIndexQuery, ILessThanEqualQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly ManagedIndex managedIndex;
        private const int pageSizeBufferMultipler = 4;

        public LessThanEqualToQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, TKey key)
        {
            this.managedIndex = managedIndex;
            Indexer = indexer;
            Key = key;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public TKey Key { get; private set; }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.OpenRangeScan; }
        }

        public double EstimatedQueryCost(Transaction transaction)
        {
            return managedIndex.Index.KeyRange(new DatabaseEntry(managedIndex.KeyAsByteArray(Key)), transaction).Less *
                   managedIndex.Index.FastStats().nPages / pageSizeBufferMultipler * (double)QueryCostScale;
        }

        public IEnumerable<Guid> Execute(Transaction transaction)
        {
            var cursor = managedIndex.Index.Cursor(new CursorConfig(), transaction);
            try
            {
                var comparer = managedIndex.Comparer;
                var bufferSize = (int)managedIndex.Index.Pagesize * pageSizeBufferMultipler;
                if(cursor.MoveFirstMultipleKey(bufferSize))
                {
                    do
                    {
                        foreach(var guid in
                            cursor.CurrentMultipleKey
                                .Select(_ => new {key = _.Key.Data, value = _.Value.Data.AsGuid()})
                                .TakeWhile(pair => comparer.Compare(managedIndex.ByteArrayAsKey(pair.key), Key) <= 0)
                                .Select(_ => _.value))
                        {
                            yield return guid;
                        }
                    }
                    while(cursor.MoveNextDuplicateMultipleKey(bufferSize));
                }
            }
            finally
            {
                cursor.Close();
            }
        }

        public IEnumerable<Guid> ExecuteInsideIntersect(Transaction transaction, IEnumerable<Guid> joinConstraint)
        {
            if(joinConstraint.Count() > EstimatedQueryCost(transaction))
                return Execute(transaction);

            var comparer = managedIndex.Comparer;

            return
                joinConstraint.Aggregate(
                    Enumerable.Empty<Guid>(),
                    (keys, guid) => keys.Union(
                        IndexMatching
                            .GetReverseMatching<TKey>(managedIndex, transaction, guid)
                            .Where(key => comparer.Compare(key, Key) <= 0)
                            .Select(_ => guid)));
        }

        public IGreaterThanQuery<TKey> GetComplementaryQuery()
        {
            return new GreaterThanQuery<TKey>(managedIndex, Indexer, Key);
        }
    }
}