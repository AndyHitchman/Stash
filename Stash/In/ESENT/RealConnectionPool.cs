namespace Stash.In.ESENT
{
    using System;
    using System.Collections.Generic;

    public class RealConnectionPool : ConnectionPool
    {
        private readonly object locker = new object();
        private readonly HashSet<PooledConnection> pooledConnections;
        private bool isDisposed;

        /// <summary>
        /// Create the connection pool, specifying the initial pool size.
        /// The pool size grows dynamically to reflect system load.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="initialPoolSize"></param>
        public RealConnectionPool(Database database, ushort initialPoolSize)
        {
            pooledConnections = new HashSet<PooledConnection>();

            for(ushort i = 0; i < initialPoolSize; i++)
            {
                pooledConnections.Add(new PooledConnection(database));
            }
        }

        /// <summary>
        /// Create the connection pool with the default pool size of 32 connections.
        /// The pool size grows dynamically to reflect system load.
        /// </summary>
        /// <param name="database"></param>
        public RealConnectionPool(Database database) : this(database, 32)
        {
        }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;

            lock(locker)
            {
                foreach(var pooledConnection in pooledConnections)
                {
                    pooledConnection.Dispose();
                }
                pooledConnections.Clear();
            }

            GC.SuppressFinalize(this);
        }

        ~RealConnectionPool()
        {
            Dispose();
        }
    }
}