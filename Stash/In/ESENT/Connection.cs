namespace Stash.In.ESENT
{
    using System;
    using Microsoft.Isam.Esent.Interop;

    public class Connection : IDisposable
    {
        private readonly Database database;
        private bool isDisposed;

        public Connection(Database database)
        {
            this.database = database;
            Session = new Session(database.Instance);
        }

        public Session Session { get; private set; }

        public Transaction BeginTransaction()
        {
            return new Transaction(Session);
        }


        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;

            GC.SuppressFinalize(this);
        }

        ~Connection()
        {
            Dispose();
        }
    }
}