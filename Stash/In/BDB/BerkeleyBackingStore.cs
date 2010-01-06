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
        private const string DbName = "stash.db";
        private readonly string databaseDirectory;
        private readonly BTreeDatabaseConfig secondaryDatabaseConfig;
        private HashDatabase db;
        private DatabaseEnvironment env;
        private bool isDisposed;


        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        /// <param name="databaseDirectory"></param>
        /// <param name="databaseEnvironmentConfig"></param>
        public BerkeleyBackingStore(
            string databaseDirectory,
            DatabaseEnvironmentConfig databaseEnvironmentConfig)
            : this(databaseDirectory, databaseEnvironmentConfig, new PrimaryDatabaseConfig(), new SecondaryDatabaseConfig())
        {
        }

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        /// <param name="databaseDirectory"></param>
        /// <param name="databaseEnvironmentConfig"></param>
        /// <param name="primaryDatabaseConfig"></param>
        /// <param name="secondaryDatabaseConfig"></param>
        public BerkeleyBackingStore(
            string databaseDirectory,
            DatabaseEnvironmentConfig databaseEnvironmentConfig,
            HashDatabaseConfig primaryDatabaseConfig,
            BTreeDatabaseConfig secondaryDatabaseConfig)
        {
            this.databaseDirectory = databaseDirectory;
            this.secondaryDatabaseConfig = secondaryDatabaseConfig;
            Directory.CreateDirectory(Path.Combine(databaseDirectory, databaseEnvironmentConfig.CreationDir));
            openDatabase(databaseDirectory, databaseEnvironmentConfig, primaryDatabaseConfig);
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

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="indexer"/>.
        /// </summary>
        /// <remarks>Indexes are stored in secordary databases.</remarks>
        /// <typeparam name="TGraph"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="indexer"></param>
        /// <returns>true is the backing store is going to manage the indexer, false if the backing store expects management to be by the Stash Engine.</returns>
        public bool EnsureIndexer<TGraph, TKey>(Indexer<TGraph, TKey> indexer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="mapper"/>.
        /// </summary>
        /// <remarks>Maps are stored in a secondary database.</remarks>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="mapper"></param>
        /// <returns>true is the backing store is going to manage the mapper, false if the backing store expects management to be by the Stash Engine.</returns>
        public bool EnsureMapper<TGraph>(Mapper<TGraph> mapper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="reducer"/>.
        /// </summary>
        /// <remarks>Reductions are stored in a secondary database.</remarks>
        /// <param name="reducer"></param>
        /// <returns>true is the backing store is going to manage the reducer, false if the backing store expects management to be by the Stash Engine.</returns>
        public bool EnsureReducer(Reducer reducer)
        {
            throw new NotImplementedException();
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

        ~BerkeleyBackingStore()
        {
            Dispose();
        }

        private void openDatabase(string rootDir, DatabaseEnvironmentConfig environmentConfig, HashDatabaseConfig databaseConfig)
        {
            env = DatabaseEnvironment.Open(rootDir, environmentConfig);
            databaseConfig.Env = env;
            db = HashDatabase.Open(DbName, databaseConfig);
        }
    }
}