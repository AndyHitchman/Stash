namespace Stash.In.BDB
{
    using BerkeleyDB;

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