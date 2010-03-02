namespace Stash.In.BDB.BerkeleyQueries
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