namespace Stash.In.BDB
{
    using BerkeleyDB;

    public class SecondaryDatabaseConfig : BTreeDatabaseConfig
    {
        public SecondaryDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.SORTED;
            FreeThreaded = true;
            ReadUncommitted = true;
        }
    }
}