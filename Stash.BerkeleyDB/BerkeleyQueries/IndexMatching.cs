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
    using Stash.Engine;

    public class IndexMatching
    {
        public static IEnumerable<InternalId> GetMatching<TKey>(ManagedIndex managedIndex, Transaction transaction, TKey key) where TKey : IComparable<TKey>
        {
            try
            {
                return managedIndex.Index.GetMultiple(new DatabaseEntry(managedIndex.KeyAsByteArray(key)), (int)managedIndex.Index.Pagesize, transaction)
                    .Value
                    .Select(graphKey => graphKey.Data.AsInternalId());
            }
            catch(NotFoundException)
            {
                return Enumerable.Empty<InternalId>();
            }
        }

        public static IEnumerable<TKey> GetReverseMatching<TKey>(ManagedIndex managedIndex, Transaction transaction, InternalId internalId) where TKey : IComparable<TKey>
        {
            try
            {
                return managedIndex.ReverseIndex.GetMultiple(new DatabaseEntry(internalId.AsByteArray()), (int)managedIndex.Index.Pagesize, transaction)
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