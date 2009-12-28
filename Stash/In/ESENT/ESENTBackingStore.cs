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
        private readonly FileInfo databasePath;
        private Instance instance;
        private JET_DBID dbid;

        /// <summary>
        /// Create an instance of the backing store implementation using ESENT
        /// </summary>
        /// <param name="databasePath"></param>
        public ESENTBackingStore(FileInfo databasePath)
        {
            this.databasePath = databasePath;
            setupOnce();
            setupInstance();
        }

        /// <summary>
        /// Create an instance of the backing store implementation using ESENT
        /// </summary>
        /// <param name="databasePath"></param>
        public ESENTBackingStore(string databasePath) : this(new FileInfo(databasePath))
        {
        }

        public void OpenDatabase()
        {
            using(var session = new Session(instance))
            {
                try
                {
                    Api.JetOpenDatabase(session, databasePath.FullName, "", out dbid, OpenDatabaseGrbit.None);
                }
                catch(EsentErrorException)
                {
                    Api.JetDetachDatabase(session, databasePath.FullName);
                    throw;
                }
            }
        }

        public void CloseDatabase()
        {
            using(var session = new Session(instance))
            {
                try
                {
                    Api.JetCloseDatabase(session, dbid, CloseDatabaseGrbit.None);
                }
                finally
                {
                    Api.JetDetachDatabase(session, databasePath.FullName);
                }
            }
        }

        private void createDatabase()
        {
            using(var session = new Session(instance))
            {
                try
                {
                    JET_DBID dbidForCreation;
                    Api.JetCreateDatabase(session, databasePath.FullName, "", out dbidForCreation, CreateDatabaseGrbit.None);

                    try
                    {
                        using(var transaction = new Transaction(session))
                        {
                            createPersistenceTable(session, dbidForCreation);
                            transaction.Commit(CommitTransactionGrbit.None);
                            Api.JetCloseDatabase(session, dbidForCreation, CloseDatabaseGrbit.None);
                        }
                    }
                    catch(EsentErrorException)
                    {
                        Api.JetCloseDatabase(session, dbidForCreation, CloseDatabaseGrbit.None);
                        throw;
                    }
                }
                catch(EsentErrorException)
                {
                    databasePath.Directory.Delete(true);
                    throw;
                }
                finally
                {
                    Api.JetDetachDatabase(session, databasePath.FullName);
                }
            }
        }

        private void createPersistenceTable(Session session, JET_DBID dbid)
        {
        }

        private void setupInstance()
        {
            instance = new Instance(Guid.NewGuid().ToString());
            instance.Parameters.SystemDirectory = Path.Combine(databasePath.DirectoryName, "System");
            instance.Parameters.TempDirectory = Path.Combine(databasePath.DirectoryName, "Temp");
            instance.Parameters.LogFileDirectory = Path.Combine(databasePath.DirectoryName, "Log");
            //In case restore has moved to location that differs from the absolute path in the log files.
            instance.Parameters.AlternateDatabaseRecoveryDirectory = databasePath.DirectoryName;
            instance.Parameters.CreatePathIfNotExist = true;
            instance.Parameters.LogFileSize = 2048;
            instance.Parameters.LogBuffers = 2048;

            instance.Init();

            if(!databasePath.Exists)
            {
                createDatabase();
            }
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
    }
}