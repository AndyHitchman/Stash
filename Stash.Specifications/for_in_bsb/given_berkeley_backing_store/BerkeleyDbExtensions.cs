namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using BerkeleyDB;
    using In.BDB;
    using NUnit.Framework;
    using Support;

    public static class BerkeleyDbExtensions
    {
        public static void ShouldHaveKeyInPrimary(this BerkeleyBackingStore store, Guid key)
        {
            var primaryDatabaseConfig = new PrimaryDatabaseConfig {Env = store.Environment, Creation = CreatePolicy.NEVER, ReadOnly = true};
            var db = HashDatabase.Open(BerkeleyBackingStore.DbName, primaryDatabaseConfig);
            try
            {
                db.Exists(new DatabaseEntry(key.ToByteArray())).ShouldBeTrue();
            }
            finally
            {
                db.Close();
            }
        }

        public static byte[] ValueForPrimaryKey(this BerkeleyBackingStore store, Guid key)
        {
            var primaryDatabaseConfig = new PrimaryDatabaseConfig {Env = store.Environment, Creation = CreatePolicy.NEVER, ReadOnly = true};
            var db = HashDatabase.Open(BerkeleyBackingStore.DbName, primaryDatabaseConfig);
            try
            {
                return db.Get(new DatabaseEntry(key.ToByteArray())).Value.Data;
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForPrimaryKey: Key not found");
            }
            finally
            {
                db.Close();
            }
            return null;
        }
    }
}