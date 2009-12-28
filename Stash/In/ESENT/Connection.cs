namespace Stash.In.ESENT
{
    using System;

    public class Connection : IDisposable
    {
        private bool isDisposed;

        public Connection(Database database)
        {
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