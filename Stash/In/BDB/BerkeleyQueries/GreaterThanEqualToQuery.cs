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

    public class GreaterThanEqualToQuery<TKey> : IBerkeleyIndexQuery, IGreaterThanEqualQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private const int pageSizeBufferMultipler = 4;

        public GreaterThanEqualToQuery(IRegisteredIndexer indexer, TKey key)
        {
            Indexer = indexer;
            Key = key;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public TKey Key { get; private set; }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.OpenRangeScan; }
        }

        public double EstimatedQueryCost(ManagedIndex managedIndex, Transaction transaction)
        {
            return managedIndex.Index.KeyRange(new DatabaseEntry(managedIndex.KeyAsByteArray(Key)), transaction).Greater *
                   managedIndex.Index.FastStats().nPages / (double)pageSizeBufferMultipler * (double)QueryCostScale;
        }

        public IEnumerable<Guid> Execute(ManagedIndex managedIndex, Transaction transaction)
        {
            var cursor = managedIndex.Index.Cursor(new CursorConfig(), transaction);
            try
            {
                var keyAsBytes = managedIndex.KeyAsByteArray(Key);
                var bufferSize = (int)managedIndex.Index.Pagesize * pageSizeBufferMultipler;
                if(cursor.MoveMultipleKey(new DatabaseEntry(keyAsBytes), false, bufferSize))
                {
                    do
                    {
                        foreach(var guid in cursor.CurrentMultipleKey.Select(_ => _.Value.Data.AsGuid()))
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

        public IEnumerable<Guid> ExecuteInsideIntersect(ManagedIndex managedIndex, Transaction transaction, IEnumerable<Guid> joinConstraint)
        {
            //TODO: Think of a better approach than simply throwing away the advantage of the other half of the intersect.
            return Execute(managedIndex, transaction);
        }

        public ILessThanQuery<TKey> GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }
    }
}