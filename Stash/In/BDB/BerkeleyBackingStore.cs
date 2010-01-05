namespace Stash.In.BDB
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BerkeleyDB;
    using Engine;

    /// <summary>
    /// A backing store for Stash using BerkeleyDB.
    /// </summary>
    public class BerkeleyBackingStore : BackingStore
    {
        private const string DataSubdirectory = "data";
        private const string DbName = "stash.db";
        private readonly string databaseDirectory;
        private HashDatabase db;
        private DatabaseEnvironment env;
        private bool isDisposed;


        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        /// <param name="databaseDirectory"></param>
        public BerkeleyBackingStore(string databaseDirectory)
        {
            this.databaseDirectory = databaseDirectory;
            Directory.CreateDirectory(Path.Combine(databaseDirectory, DataSubdirectory));
            openDatabase(databaseDirectory);
        }

        public void DeleteGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if(isDisposed) return;

            isDisposed = true;
            closeDatabase();
        }

        ~BerkeleyBackingStore()
        {
            Dispose();
        }

        public IEnumerable<PersistentGraph> GetExternallyModifiedGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersistentGraph> GetGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public void InsertGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        public void UpdateGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        private void closeDatabase()
        {
            db.Close();
            env.Close();
            DatabaseEnvironment.Remove(databaseDirectory);
        }

        private void openDatabase(string rootDir)
        {
            var dataDir = Path.Combine(rootDir, DataSubdirectory);

            var envConfig = new DatabaseEnvironmentConfig
                                {
                                    MPoolSystemCfg = new MPoolConfig {CacheSize = new CacheInfo(0, 128*1024, 1)},
                                    Create = true,
                                    CreationDir = dataDir,
                                    ErrorPrefix = "Stash",
                                    UseLogging = true,
                                    UseLocking = true,
                                    UseMPool = true,
                                    UseTxns = true,
                                    FreeThreaded = true,
                                    RunRecovery = true,
                                    LockSystemCfg = new LockingConfig {DeadlockResolution = DeadlockPolicy.MIN_WRITE}
                                };
            envConfig.DataDirs.Add(dataDir);

            env = DatabaseEnvironment.Open(rootDir, envConfig);

            var dbConfig = new HashDatabaseConfig
                               {
                                   AutoCommit = true,
                                   Creation = CreatePolicy.IF_NEEDED,
                                   Duplicates = DuplicatesPolicy.NONE,
                                   FreeThreaded = true,
                                   ReadUncommitted = true,
                                   Env = env
                               };

            db = HashDatabase.Open(DbName, dbConfig);
        }
    }
}