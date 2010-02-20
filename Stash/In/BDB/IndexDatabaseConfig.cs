namespace Stash.In.BDB
{
    using BerkeleyDB;

    /// <summary>
    /// We can't use secondary databases for our indexes because the .NET wrapper only supports
    /// key generation functions that return a single value.
    /// </summary>
    public class IndexDatabaseConfig : BTreeDatabaseConfig
    {
        public IndexDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.SORTED;
            FreeThreaded = true;
            ReadUncommitted = true;
            AutoCommit = true;
        }
    }
}