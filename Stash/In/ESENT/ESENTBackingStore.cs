namespace Stash.In.ESENT
{
    using System;
    using System.IO;
    using Configuration;
    using Microsoft.Isam.Esent.Interop;

    public class ESENTBackingStore : BackingStore
    {
        private static readonly object setupLocker = new object();
        private static bool isSetup;
        private bool isDisposed;    

        /// <summary>
        /// Create an instance of the backing store implementation using ESENT
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="connectionPool"></param>
        public ESENTBackingStore(FileInfo databasePath, ConnectionPool connectionPool)
        {
            setupOnce();
            Database = new Database(setupInstance(databasePath), databasePath.FullName, connectionPool);

            if (!databasePath.Exists)
            {
                createDatabase();
            }
        }

        /// <summary>
        /// Create an instance of the backing store implementation using ESENT
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="connectionPool"></param>
        public ESENTBackingStore(string databasePath, ConnectionPool connectionPool) : this(new FileInfo(databasePath), connectionPool)
        {
        }

        /// <summary>
        /// The initialised instance of the database.
        /// </summary>
        public Database Database { get; private set; }

        private void createDatabase()
        {
            using(var session = new Session(Database.Instance))
            {
                try
                {
                    JET_DBID dbid;
                    Api.JetCreateDatabase(session, Database.Path, "", out dbid, CreateDatabaseGrbit.None);

                    try
                    {
                        using(var transaction = new Transaction(session))
                        {
                            createPersistenceTable(session, dbid);
                            transaction.Commit(CommitTransactionGrbit.None);
                            Api.JetCloseDatabase(session, dbid, CloseDatabaseGrbit.None);
                        }
                    }
                    catch(EsentErrorException)
                    {
                        Api.JetCloseDatabase(session, dbid, CloseDatabaseGrbit.None);
                        throw;
                    }
                }
                catch(EsentErrorException)
                {
                    new FileInfo(Database.Path).Directory.Delete(true);
                    throw;
                }
                finally
                {
                    Api.JetDetachDatabase(session, Database.Path);
                }
            }
        }

        private void createPersistenceTable(Session session, JET_DBID dbid)
        {
        }

        private Instance setupInstance(FileInfo databasePath)
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

        private void setupOnce()
        {
            if(!isSetup)
                lock(setupLocker)
                    if(!isSetup)
                    {
                        isSetup = true;
                        SystemParameters.DatabasePageSize = 8192;
                        SystemParameters.Configuration = 0;
                        SystemParameters.EnableAdvanced = true;
                    }
        }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;
            
            Database.Dispose();
            
            GC.SuppressFinalize(this);
        }

        ~ESENTBackingStore()
        {
            Dispose();
        }
    }
}