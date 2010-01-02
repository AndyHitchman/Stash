namespace Stash.In.ESENT
{
    using System;
    using System.Globalization;
    using System.IO;
    using Configuration;
    using Microsoft.Isam.Esent.Interop;

    public class ESENTBackingStore : BackingStore
    {
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
            Database = new Database(setupInstance(databasePath), databasePath.FullName, connectionPool);

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