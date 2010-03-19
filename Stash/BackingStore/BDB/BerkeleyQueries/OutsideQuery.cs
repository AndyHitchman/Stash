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

    public class OutsideQuery<TKey> : IBerkeleyIndexQuery, IOutsideQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private const int pageSizeBufferMultipler = 4;
        private readonly ManagedIndex managedIndex;

        public OutsideQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey)
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
            get { return QueryCostScale.FullScan; }
        }

        public double EstimatedQueryCost(Transaction transaction)
        {
            return managedIndex.Index.FastStats().nPages / (double)pageSizeBufferMultipler * (double)QueryCostScale;
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
                        var firstKey = managedIndex.ByteArrayAsKey(cursor.CurrentMultipleKey.First().Key.Data);
                        var lower = comparer.Compare(firstKey, LowerKey) < 0;
                        var higher = comparer.Compare(firstKey, UpperKey) > 0;

                        if(lower)
                            foreach(var guid in
                                cursor.CurrentMultipleKey
                                    .Select(_ => new {key = _.Key.Data, value = _.Value.Data.AsGuid()})
                                    .TakeWhile(pair => comparer.Compare(managedIndex.ByteArrayAsKey(pair.key), LowerKey) < 0)
                                    .Select(_ => _.value))
                            {
                                yield return guid;
                            }

                        if(!higher)
                            foreach(var guid in
                                cursor.CurrentMultipleKey
                                    .Select(_ => new {key = _.Key.Data, value = _.Value.Data.AsGuid()})
                                    .SkipWhile(pair => comparer.Compare(managedIndex.ByteArrayAsKey(pair.key), UpperKey) <= 0)
                                    .Select(_ => _.value))
                            {
                                yield return guid;
                            }

                        if(higher)
                            foreach(var guid in
                                cursor.CurrentMultipleKey
                                    .Select(_ => _.Value.Data.AsGuid()))
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
                            .Where(key => comparer.Compare(key, LowerKey) < 0 || comparer.Compare(key, UpperKey) > 0)
                            .Select(_ => guid)));
        }
    }
}