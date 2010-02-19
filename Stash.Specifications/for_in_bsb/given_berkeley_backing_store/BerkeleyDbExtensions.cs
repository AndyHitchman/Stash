namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using BerkeleyDB;
    using In.BDB;
    using NUnit.Framework;
    using Support;

    public static class BerkeleyDbExtensions
    {
        public static void ShouldHaveKeyInPrimary(this HashDatabase store, Guid key)
        {
            store.Exists(new DatabaseEntry(key.ToByteArray())).ShouldBeTrue();
        }

        public static byte[] ValueForKey(this HashDatabase store, Guid key)
        {
            try
            {
                return store.Get(new DatabaseEntry(key.ToByteArray())).Value.Data;
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForKey: Key not found");
            }
            return null;
        }

        public static byte[] ValueForKey(this BTreeDatabase store, Guid key)
        {
            try
            {
                return store.Get(new DatabaseEntry(key.ToByteArray())).Value.Data;
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForKey: Key not found");
            }
            return null;
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