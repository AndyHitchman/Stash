namespace Stash.In.ESENT
{
    using System;

    public class PooledConnection : IDisposable
    {
        private bool isDisposed;

        public PooledConnection(Database database)
        {
            Connection = new Connection(database);
        }

        public Connection Connection { get; private set; }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;
            
            Connection.Dispose();
            
            GC.SuppressFinalize(this);
        }

        ~PooledConnection()
        {
            Dispose();
        }
    }
}