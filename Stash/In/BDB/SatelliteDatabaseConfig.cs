namespace Stash.In.BDB
{
    using BerkeleyDB;

    public class SatelliteDatabaseConfig : BTreeDatabaseConfig
    {
        public SatelliteDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.SORTED;
            FreeThreaded = true;
            ReadUncommitted = true;
            AutoCommit = true;
        }
    }
}