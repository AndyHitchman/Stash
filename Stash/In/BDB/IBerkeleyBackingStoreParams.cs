namespace Stash.In.BDB
{
    using BerkeleyDB;

    public interface IBerkeleyBackingStoreParams
    {
        string DatabaseDirectory { get; }
        DatabaseEnvironmentConfig DatabaseEnvironmentConfig { get; }
        HashDatabaseConfig GraphDatabaseConfig { get; }
        BTreeDatabaseConfig SatelliteDatabaseConfig { get; }
    }
}