namespace Stash.In.ESENT
{
    using System;
    using Microsoft.Isam.Esent.Interop;

    public class Database : IDisposable
    {
        private bool isDisposed;

        public Database(Instance instance, string databasePath, ConnectionPool connectionPool)
        {
            Instance = instance;
            Path = databasePath;
            ConnectionPool = connectionPool;
        }

        public Instance Instance { get; private set; }
        public string Path { get; private set; }
        public ConnectionPool ConnectionPool { get; private set; }


        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;
            
            ConnectionPool.Dispose();
            Instance.Term();
            Instance.Dispose();
            
            GC.SuppressFinalize(this);
        }

        ~Database()
        {
            Dispose();
        }
    }
}