namespace Stash.In.BDB
{
    using BerkeleyDB;

    public class BerkeleyBackingStoreParams : IBerkeleyBackingStoreParams
    {
        public BerkeleyBackingStoreParams(
            string databaseDirectory,
            DatabaseEnvironmentConfig databaseEnvironmentConfig,
            HashDatabaseConfig primaryDatabaseConfig,
            BTreeDatabaseConfig secondaryDatabaseConfig)
        {
            DatabaseDirectory = databaseDirectory;
            DatabaseEnvironmentConfig = databaseEnvironmentConfig;
            ValueDatabaseConfig = primaryDatabaseConfig;
            IndexDatabaseConfig = secondaryDatabaseConfig;
        }

        public string DatabaseDirectory { get; private set; }
        public DatabaseEnvironmentConfig DatabaseEnvironmentConfig { get; private set; }
        public HashDatabaseConfig ValueDatabaseConfig { get; private set; }
        public BTreeDatabaseConfig IndexDatabaseConfig { get; private set; }
    }
}