namespace Stash.In.BDB
{
    using BerkeleyDB;

    public class PrimaryDatabaseConfig : HashDatabaseConfig
    {
        public PrimaryDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.NONE;
            FreeThreaded = true;
            ReadUncommitted = true;
        }
    }
}