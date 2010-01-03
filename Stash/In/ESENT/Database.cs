namespace Stash.In.ESENT
{
    using System;
    using Microsoft.Isam.Esent.Interop;

    public class Database : IDisposable
    {
        private bool isDisposed;

        public Database(Instance instance, string databasePath)
        {
            Schema = new Schema();
            Instance = instance;
            Path = databasePath;
        }

        public Schema Schema { get; private set; }

        public Instance Instance { get; private set; }
        public string Path { get; private set; }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;

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