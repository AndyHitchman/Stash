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
        private readonly string rootDir;
        private readonly FileInfo databasePath;
        private bool isDisposed;


        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        /// <param name="databaseDirectory"></param>
        public BerkeleyBackingStore(string databaseDirectory)
        {
            rootDir = databaseDirectory;
            if (!Directory.Exists(rootDir))
                Directory.CreateDirectory(rootDir);

            setupEnvironment();
        }

        private void setupEnvironment()
        {
            var dataDir = Path.Combine(rootDir, "data");

            /* Configure an environment. */
            var envConfig = new DatabaseEnvironmentConfig
                                {
                                    MPoolSystemCfg = new MPoolConfig {CacheSize = new CacheInfo(0, 64*1024, 1)},
                                    Create = true,
                                    CreationDir = dataDir,
                                    ErrorPrefix = "Stash",
                                    UseLogging = true,
                                    UseLocking = true,
                                    UseMPool = true,
                                    UseTxns = true
                                };
            envConfig.DataDirs.Add(dataDir);

            /* Create and open the environment. */
            var env = DatabaseEnvironment.Open(rootDir, envConfig);

            env.Close();
        }

        public void Dispose()
        {
            if(isDisposed) return;

            isDisposed = true;
            DatabaseEnvironment.Remove(rootDir);
        }

        public void InsertGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        public void UpdateGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        public void DeleteGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersistentGraph> GetGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersistentGraph> GetExternallyModifiedGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }
    }
}