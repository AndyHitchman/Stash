namespace Stash.In.BDB
{
    using BerkeleyDB;

    public class DefaultDatabaseEnvironmentConfig : DatabaseEnvironmentConfig
    {
        public DefaultDatabaseEnvironmentConfig()
        {
            var dataDir = "data";

            MPoolSystemCfg = new MPoolConfig {CacheSize = new CacheInfo(0, 128*1024, 1)};
            Create = true;
            CreationDir = dataDir;
            ErrorPrefix = "stash";
            UseLogging = true;
            UseLocking = true;
            UseMPool = true;
            UseTxns = true;
            FreeThreaded = true;
            RunRecovery = true;
            LockSystemCfg = new LockingConfig {DeadlockResolution = DeadlockPolicy.MIN_WRITE};

            DataDirs.Add(dataDir);
        }
    }
}