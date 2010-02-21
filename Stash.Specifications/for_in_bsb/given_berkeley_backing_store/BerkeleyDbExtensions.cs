namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using In.BDB;
    using NUnit.Framework;
    using Support;

    public static class BerkeleyDbExtensions
    {
        public static void ShouldHaveKey(this HashDatabase store, Guid key)
        {
            store.Exists(new DatabaseEntry(key.AsByteArray())).ShouldBeTrue();
        }

        public static byte[] ValueForKey(this HashDatabase store, Guid key)
        {
            try
            {
                return store.Get(new DatabaseEntry(key.AsByteArray())).Value.Data;
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForKey: Key not found");
            }
            return null;
        }

        public static IEnumerable<byte[]> ValuesForKey(this BTreeDatabase store, object key)
        {
            return ValuesForKey(store, key.ToString().AsByteArray());
        }

        public static IEnumerable<byte[]> ValuesForKey(this BTreeDatabase store, byte[] key)
        {
            try
            {
                return store.GetMultiple(new DatabaseEntry(key)).Value.Select(_ => _.Data);
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForKey: Key not found");
            }
            return null;
        }

        public static byte[] ValueForKey(this BTreeDatabase store, object key)
        {
            return ValueForKey(store, key.ToString().AsByteArray());
        }

        public static byte[] ValueForKey(this BTreeDatabase store, byte[] key)
        {
            try
            {
                return store.Get(new DatabaseEntry(key)).Value.Data;
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForKey: Key not found");
            }
            return null;
        }
    }
}