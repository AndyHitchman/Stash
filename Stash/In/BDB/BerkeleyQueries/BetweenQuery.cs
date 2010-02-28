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

    public class BetweenQuery<TKey> : IBerkeleyQuery, IBetweenQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private const int pageSizeBufferMultipler = 32;

        public BetweenQuery(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey)
        {
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

        public double EstimatedQueryCost(ManagedIndex managedIndex, Transaction transaction)
        {
            return (managedIndex.Index.KeyRange(new DatabaseEntry(managedIndex.KeyAsByteArray(UpperKey)), transaction).Less -
                    managedIndex.Index.KeyRange(new DatabaseEntry(managedIndex.KeyAsByteArray(LowerKey)), transaction).Greater) *
                   managedIndex.Index.FastStats().nPages *
                   (double)QueryCostScale;
        }

        public IEnumerable<Guid> Execute(ManagedIndex managedIndex, Transaction transaction)
        {
            var cursor = managedIndex.Index.Cursor(new CursorConfig(), transaction);
            try
            {
                var comparer = managedIndex.Comparer;
                var keyAsBytes = managedIndex.KeyAsByteArray(LowerKey);
                var bufferSize = (int)managedIndex.Index.Pagesize * pageSizeBufferMultipler;
                if(cursor.MoveMultipleKey(new DatabaseEntry(keyAsBytes), false, bufferSize))
                {
                    do
                    {
                        foreach(var guid in
                            cursor.CurrentMultipleKey
                                .Select(_ => new {key = _.Key.Data, value = _.Value.Data.AsGuid()})
                                .TakeWhile(pair => comparer.Compare(managedIndex.ByteArrayAsKey(pair.key), UpperKey) <= 0)
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
    }
}