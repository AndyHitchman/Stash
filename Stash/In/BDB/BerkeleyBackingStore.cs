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
        /// Ensures that the database is configured to persist the provided <paramref name="index"/>.
        /// </summary>
        /// <remarks>Indexes are stored in secordary databases.</remarks>
        /// <typeparam name="TGraph"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="index"></param>
        /// <returns>true is the backing store is going to manage the Index, false if the backing store expects management to be by the Stash Engine.</returns>
        public bool EnsureIndex<TGraph, TKey>(Index<TGraph, TKey> index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="map"/>.
        /// </summary>
        /// <remarks>Maps are stored in a secondary database.</remarks>
        /// <typeparam name="TGraph"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map"></param>
        /// <returns>true is the backing store is going to manage the Map, false if the backing store expects management to be by the Stash Engine.</returns>
        public bool EnsureMap<TGraph,TKey,TValue>(Map<TGraph,TKey,TValue> map)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="reduction"/>.
        /// </summary>
        /// <remarks>Reductions are stored in a secondary database.</remarks>
        /// <param name="reduction"></param>
        /// <returns>true is the backing store is going to manage the Reduction, false if the backing store expects management to be by the Stash Engine.</returns>
        public bool EnsureReduction(Reduction reduction)
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