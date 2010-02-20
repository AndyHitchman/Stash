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
    public class BerkeleyBackingStore : IBackingStore, IDisposable
    {
        private const string ConcreteTypeFileName = "concreteTypes.db";
        private const string GraphFileName = "graphs.db";
        private const string IndexFilenamePrefix = "index-";
        private const string TypeHierarchyFileName = "typeHierarchy.db";

        private readonly IBerkeleyBackingStoreParams backingStoreParams;
        private bool isDisposed;

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        public BerkeleyBackingStore(IBerkeleyBackingStoreParams backingStoreParams)
        {
            this.backingStoreParams = backingStoreParams;
            Directory.CreateDirectory(Path.Combine(backingStoreParams.DatabaseDirectory, backingStoreParams.DatabaseEnvironmentConfig.CreationDir));
            IndexDatabases = new Dictionary<string, IndexManager>();

            dbOpen();
        }

        public DatabaseEnvironment Environment { get; private set; }

        public HashDatabase GraphDatabase { get; private set; }
        public HashDatabase ConcreteTypeDatabase { get; private set; }
        public BTreeDatabase TypeHierarchyDatabase { get; private set; }
        public Dictionary<string, IndexManager> IndexDatabases { get; private set; }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;

            dbClose();
        }

        public void EnsureIndex(string indexName, Type yieldsType)
        {
            var indexDatabase = BTreeDatabase.Open(IndexFilenamePrefix + indexName + ".db", backingStoreParams.IndexDatabaseConfig);
            IndexDatabases.Add(indexName, new IndexManager(indexName, yieldsType, indexDatabase));
        }

        public IStoredGraph Get(Guid internalId)
        {
            throw new NotImplementedException();
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            var storageWork = new BerkeleyStorageWork(this);
            storageWorkActions(storageWork);
            storageWork.Commit();
        }

        ~BerkeleyBackingStore()
        {
            Dispose();
        }

        private void closeConcreteTypeDatabase()
        {
            if(ConcreteTypeDatabase != null) ConcreteTypeDatabase.Close();
        }

        private void closeEnvironment()
        {
            if(Environment != null) Environment.Close();
            DatabaseEnvironment.Remove(backingStoreParams.DatabaseDirectory);
        }

        private void closeGraphDatabase()
        {
            if(GraphDatabase != null) GraphDatabase.Close();
        }

        private void closeIndexDatabases()
        {
            foreach(var indexManager in IndexDatabases.Values)
            {
                indexManager.Close();
            }
        }

        private void closeTypeHierarchyDatabase()
        {
            if(TypeHierarchyDatabase != null) TypeHierarchyDatabase.Close();
        }

        private void dbClose()
        {
            closeIndexDatabases();
            closeTypeHierarchyDatabase();
            closeConcreteTypeDatabase();
            closeGraphDatabase();

            closeEnvironment();
        }

        private void dbOpen()
        {
            openEnvironment();

            openGraphDatabase();
            openConcreteTypeDatabase();
            openTypeHierarchyDatabase();
        }

        private void openConcreteTypeDatabase()
        {
            ConcreteTypeDatabase = HashDatabase.Open(ConcreteTypeFileName, backingStoreParams.ValueDatabaseConfig);
        }

        private void openEnvironment()
        {
            Environment = DatabaseEnvironment.Open(backingStoreParams.DatabaseDirectory, backingStoreParams.DatabaseEnvironmentConfig);
            backingStoreParams.ValueDatabaseConfig.Env = Environment;
            backingStoreParams.IndexDatabaseConfig.Env = Environment;
        }

        private void openGraphDatabase()
        {
            GraphDatabase = HashDatabase.Open(GraphFileName, backingStoreParams.ValueDatabaseConfig);
        }

        private void openTypeHierarchyDatabase()
        {
            TypeHierarchyDatabase = BTreeDatabase.Open(TypeHierarchyFileName, backingStoreParams.IndexDatabaseConfig);
        }
    }
}