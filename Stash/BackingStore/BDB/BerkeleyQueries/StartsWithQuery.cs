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

namespace Stash.BackingStore.BDB.BerkeleyQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using Configuration;
    using Queries;

    public class StartsWithQuery : IBerkeleyIndexQuery, IStartsWithQuery
    {
        private const int pageSizeBufferMultipler = 4;
        private readonly ManagedIndex managedIndex;

        public StartsWithQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, string startsWith)
        {
            this.managedIndex = managedIndex;
            Indexer = indexer;
            Key = startsWith;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public string Key { get; private set; }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.ClosedRangeScan; }
        }

        public double EstimatedQueryCost(Transaction transaction)
        {
            var endOfKeyRange = string.Concat(Key, new string(char.MaxValue, 1));
            return (managedIndex.Index.KeyRange(new DatabaseEntry(managedIndex.KeyAsByteArray(endOfKeyRange)), transaction).Less -
                    managedIndex.Index.KeyRange(new DatabaseEntry(managedIndex.KeyAsByteArray(Key)), transaction).Less) *
                   managedIndex.Index.FastStats().nPages / pageSizeBufferMultipler * (double)QueryCostScale;
        }

        public IEnumerable<Guid> Execute(Transaction transaction)
        {
            var cursor = managedIndex.Index.Cursor(new CursorConfig(), transaction);
            try
            {
                var comparer = managedIndex.Comparer;
                var keyAsBytes = managedIndex.KeyAsByteArray(Key);
                var bufferSize = (int)managedIndex.Index.Pagesize * pageSizeBufferMultipler;

                if(cursor.MoveMultipleKey(new DatabaseEntry(keyAsBytes), false, bufferSize))
                {
                    do
                    {
                        //Each char is 2 bytes. Only take the start of the key data to the length of the query key.
                        foreach(var guid in
                            cursor.CurrentMultipleKey
                                .Select(_ => new {key = _.Key.Data.Take(Key.Length * 2).ToArray(), value = _.Value.Data.AsGuid()})
                                .TakeWhile(pair => comparer.Compare(managedIndex.ByteArrayAsKey(pair.key), Key) == 0)
                                .Select(_ => _.value))
                        {
                            yield return guid;
                        }
                    }
                    while(cursor.MoveNextMultipleKey(bufferSize) &&
                          comparer.Compare(managedIndex.ByteArrayAsKey(cursor.CurrentMultipleKey.First().Key.Data), Key) == 0);
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

            return
                joinConstraint.Aggregate(
                    Enumerable.Empty<Guid>(),
                    (matching, joinMatched) => matching.Union(
                        IndexMatching
                            .GetReverseMatching<string>(managedIndex, transaction, joinMatched)
                            .Where(key => key.StartsWith(Key))
                            .Select(_ => joinMatched)));
        }
    }
}