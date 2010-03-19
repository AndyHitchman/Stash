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

    public class IndexMatching
    {
        public static IEnumerable<Guid> GetMatching<TKey>(ManagedIndex managedIndex, Transaction transaction, TKey key) where TKey : IComparable<TKey>
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

        public static IEnumerable<TKey> GetReverseMatching<TKey>(ManagedIndex managedIndex, Transaction transaction, Guid guid) where TKey : IComparable<TKey>
        {
            try
            {
                return managedIndex.ReverseIndex.GetMultiple(new DatabaseEntry(guid.AsByteArray()), (int)managedIndex.Index.Pagesize, transaction)
                    .Value
                    .Select(indexKey => (TKey)managedIndex.ByteArrayAsKey(indexKey.Data));
            }
            catch(NotFoundException)
            {
                return Enumerable.Empty<TKey>();
            }
        }
    }
}