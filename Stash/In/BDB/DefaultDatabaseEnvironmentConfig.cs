namespace Stash.In.BDB
{
    using BerkeleyDB;

    public class DefaultDatabaseEnvironmentConfig : DatabaseEnvironmentConfig
    {
        public DefaultDatabaseEnvironmentConfig()
        {
            const string dataDir = "data";

            AutoCommit = false;
            Create = true;
            ErrorPrefix = "stash";
            UseLogging = true;
            UseTxns = true;
            FreeThreaded = true;
            RunRecovery = true;

            UseMPool = true;
            MPoolSystemCfg = new MPoolConfig {CacheSize = new CacheInfo(0, 128*1024, 1)};

            UseLocking = true;
            LockSystemCfg = new LockingConfig {DeadlockResolution = DeadlockPolicy.MIN_WRITE};

            CreationDir = dataDir;
            DataDirs.Add(dataDir);
        }
    }
}