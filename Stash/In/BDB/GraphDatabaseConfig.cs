namespace Stash.In.BDB
{
    using BerkeleyDB;

    public class GraphDatabaseConfig : HashDatabaseConfig
    {
        public GraphDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.NONE;
            FreeThreaded = true;
            ReadUncommitted = true;
            AutoCommit = true;
        }
    }
}