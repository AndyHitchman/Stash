namespace Stash.In.BDB.Configuration
{
    using System;
    using BerkeleyDB;

    /// <summary>
    /// We can't use secondary databases for our indexes because the .NET wrapper only supports
    /// key generation functions that return a single value.
    /// </summary>
    public abstract class IndexDatabaseConfig : BTreeDatabaseConfig
    {
        public IndexDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.SORTED;
            FreeThreaded = true;
            ReadUncommitted = true;
            AutoCommit = true;
        }

        public abstract Byte[] AsByteArray(object key);
    }
}