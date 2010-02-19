namespace Stash.In.BDB
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BerkeleyDB;
    using Engine;
    using System.Linq;

    /// <summary>
    /// A backing store for Stash using BerkeleyDB.
    /// </summary>
    public class BerkeleyBackingStore : IBackingStore, IDisposable
    {
        public const string DbName = "stash.db";
        private readonly string databaseDirectory;
        private readonly BTreeDatabaseConfig secondaryDatabaseConfig;
        private HashDatabase db;
        public DatabaseEnvironment Environment { get; set; }
        private bool isDisposed;

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        public BerkeleyBackingStore(IBerkeleyBackingStoreParams berkeleyBackingStoreParams)
        {
            databaseDirectory = berkeleyBackingStoreParams.DatabaseDirectory;
            secondaryDatabaseConfig = berkeleyBackingStoreParams.SecondaryDatabaseConfig;
            Directory.CreateDirectory(Path.Combine(berkeleyBackingStoreParams.DatabaseDirectory, berkeleyBackingStoreParams.DatabaseEnvironmentConfig.CreationDir));
            openDatabase(
                            berkeleyBackingStoreParams.DatabaseDirectory,
                            berkeleyBackingStoreParams.DatabaseEnvironmentConfig,
                            berkeleyBackingStoreParams.PrimaryDatabaseConfig);
        }


        public void Dispose()
        {
            if(isDisposed) return;

            isDisposed = true;
            closeDatabase();
        }

        private void closeDatabase()
        {
            if(db != null) db.Close();
            if(Environment != null)
            {
                Environment.Checkpoint();
                Environment.Close();
            }
            DatabaseEnvironment.Remove(databaseDirectory);
        }

        ~BerkeleyBackingStore()
        {
            Dispose();
        }

        private void openDatabase(string rootDir, DatabaseEnvironmentConfig environmentConfig, HashDatabaseConfig databaseConfig)
        {
            Environment = DatabaseEnvironment.Open(rootDir, environmentConfig);
            databaseConfig.Env = Environment;
            db = HashDatabase.Open(DbName, databaseConfig);
        }

        public void InsertGraph(ITrackedGraph trackedGraph)
        {
            db.PutNoOverwrite(new DatabaseEntry(trackedGraph.InternalId.ToByteArray()), new DatabaseEntry(trackedGraph.SerialisedGraph.ToArray()));
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            throw new NotImplementedException();
        }

        public void DeleteGraph(Guid internalId)
        {
            throw new NotImplementedException();
        }

        public IStoredGraph Get(Guid internalId)
        {
            throw new NotImplementedException();
        }
    }
}