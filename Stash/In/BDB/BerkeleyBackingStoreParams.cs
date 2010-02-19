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
            GraphDatabaseConfig = primaryDatabaseConfig;
            SatelliteDatabaseConfig = secondaryDatabaseConfig;
        }

        public string DatabaseDirectory { get; private set; }
        public DatabaseEnvironmentConfig DatabaseEnvironmentConfig { get; private set; }
        public HashDatabaseConfig GraphDatabaseConfig { get; private set; }
        public BTreeDatabaseConfig SatelliteDatabaseConfig { get; private set; }
    }
}