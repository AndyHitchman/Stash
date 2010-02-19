namespace Stash.In.BDB
{
    using BerkeleyDB;

    public class ValueDatabaseConfig : HashDatabaseConfig
    {
        public ValueDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.NONE;
            FreeThreaded = true;
            ReadUncommitted = true;
            AutoCommit = true;
        }
    }
}