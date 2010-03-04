namespace Stash.In.ESENT
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Engine;
    using Microsoft.Isam.Esent.Interop;

    public class ESENTBackingStore : IBackingStore
    {
        private readonly ConnectionPool connectionPool;
        private bool isDisposed;

        static ESENTBackingStore()
        {
            setupOnce();
        }

        /// <summary>
        /// Create an instance of the backing store implementation using ESENT
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="connectionPool"></param>
        public ESENTBackingStore(FileInfo databasePath, ConnectionPool connectionPool)
        {
            this.connectionPool = connectionPool;
            Database = new Database(setupInstance(databasePath), databasePath.FullName);

            if(!databasePath.Exists)
            {
                SchemaManager.CreateDatabase(Database);
            }
        }

        /// <summary>
        /// Create an instance of the backing store implementation using ESENT
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="connectionPool"></param>
        public ESENTBackingStore(string databasePath, ConnectionPool connectionPool)
            : this(new FileInfo(databasePath), connectionPool)
        {
        }

        /// <summary>
        /// The initialised instance of the database.
        /// </summary>
        public Database Database { get; private set; }

        public void DeleteGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;

            Database.Dispose();

            GC.SuppressFinalize(this);
        }

        public bool EnsureIndex<TGraph, TKey>(Index<TGraph, TKey> index)
        {
            throw new NotImplementedException();
        }

        public bool EnsureMap<TGraph,TKey,TValue>(Map<TGraph,TKey,TValue> map)
        {
            throw new NotImplementedException();
        }

        public bool EnsureReduction<TKey, TValue>(Reduction<TKey, TValue> reduction)
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
            if(persistentGraphs == null) throw new ArgumentNullException("persistentGraphs");
            if(persistentGraphs.Any(graph => graph == null))
                throw new ArgumentNullException("persistentGraphs", "A persistent graph in the set was null");

            connectionPool.WithConnection(
                connection =>
                    {
                        using(var transaction = connection.BeginTransaction())
                        {
//                            if (connection.TrySeek(item.Key))
//                            {
//                                throw new ArgumentException("An item with this key already exists", "key");
//                            }

//                            using (var update = new Update(connection.Session, this.dataTable, JET_prep.Insert))
//                            {
//                                this.SetKeyColumn(data.Key);
//                                this.SetValue(data.Value);
//                                update.Save();
//                            }

                            transaction.Commit(CommitTransactionGrbit.LazyFlush);
                        }
                    });
        }

        public void UpdateGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        ~ESENTBackingStore()
        {
            Dispose();
        }


        private static Instance setupInstance(FileInfo databasePath)
        {
            var instance = new Instance(Guid.NewGuid().ToString());
            instance.Parameters.SystemDirectory = Path.Combine(databasePath.DirectoryName, "System");
            instance.Parameters.TempDirectory = Path.Combine(databasePath.DirectoryName, "Temp");
            instance.Parameters.LogFileDirectory = Path.Combine(databasePath.DirectoryName, "Log");
            //In case restore has moved to location that differs from the absolute path in the log files.
            instance.Parameters.AlternateDatabaseRecoveryDirectory = databasePath.DirectoryName;
            instance.Parameters.CreatePathIfNotExist = true;
            instance.Parameters.LogFileSize = 2048;
            instance.Parameters.LogBuffers = 2048;

            instance.Init();

            return instance;
        }

        private static void setupOnce()
        {
            SystemParameters.DatabasePageSize = 8192;
            SystemParameters.Configuration = 0;
            SystemParameters.EnableAdvanced = true;
        }
    }
}