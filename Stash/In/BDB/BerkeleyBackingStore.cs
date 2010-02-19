namespace Stash.In.BDB
{
    using System;
    using System.IO;
    using BerkeleyDB;
    using Engine;

    /// <summary>
    /// A backing store for Stash using BerkeleyDB.
    /// </summary>
    public class BerkeleyBackingStore : IBackingStore, IDisposable
    {
        private const string ConcreteTypeDbName = "concreteTypes.db";
        private const string GraphDbName = "graphs.db";
        private readonly IBerkeleyBackingStoreParams backingStoreParams;
        private bool isDisposed;

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        public BerkeleyBackingStore(IBerkeleyBackingStoreParams backingStoreParams)
        {
            this.backingStoreParams = backingStoreParams;
            Directory.CreateDirectory(Path.Combine(backingStoreParams.DatabaseDirectory, backingStoreParams.DatabaseEnvironmentConfig.CreationDir));

            dbOpen();
        }

        public HashDatabase GraphDatabase { get; private set; }
        public BTreeDatabase ConcreteTypeDatabase { get; private set; }
        public DatabaseEnvironment Environment { get; private set; }

        public void Dispose()
        {
            if(isDisposed) return;

            isDisposed = true;

            dbClose();
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

        private void dbClose()
        {
            closeConcreteTypeDatabase();
            closeGraphDatabase();

            closeEnvironment();
        }

        private void dbOpen()
        {
            openEnvironment();

            openGraphDatabase();
            openConcreteTypeDatabase();
        }

        private void openConcreteTypeDatabase()
        {
            ConcreteTypeDatabase = BTreeDatabase.Open(ConcreteTypeDbName, backingStoreParams.SatelliteDatabaseConfig);
        }

        private void openEnvironment()
        {
            Environment = DatabaseEnvironment.Open(backingStoreParams.DatabaseDirectory, backingStoreParams.DatabaseEnvironmentConfig);
            backingStoreParams.GraphDatabaseConfig.Env = Environment;
            backingStoreParams.SatelliteDatabaseConfig.Env = Environment;
        }

        private void openGraphDatabase()
        {
            GraphDatabase = HashDatabase.Open(GraphDbName, backingStoreParams.GraphDatabaseConfig);
        }
    }
}