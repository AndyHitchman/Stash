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
            PrimaryDatabaseConfig = primaryDatabaseConfig;
            SecondaryDatabaseConfig = secondaryDatabaseConfig;
        }

        public string DatabaseDirectory { get; private set; }
        public DatabaseEnvironmentConfig DatabaseEnvironmentConfig { get; private set; }
        public HashDatabaseConfig PrimaryDatabaseConfig { get; private set; }
        public BTreeDatabaseConfig SecondaryDatabaseConfig { get; private set; }
    }
}