namespace Stash.In.ESENT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RealConnectionPool : ConnectionPool
    {
        private readonly Database database;
        private readonly object locker = new object();
        private bool isDisposed;
        public readonly Queue<Connection> PooledConnections;

        /// <summary>
        /// Create the connection pool, specifying the initial pool size.
        /// The pool size grows dynamically to reflect system load.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="initialPoolSize"></param>
        public RealConnectionPool(Database database, ushort initialPoolSize)
        {
            this.database = database;
            PooledConnections = new Queue<Connection>(initialPoolSize*3);

            for(ushort i = 0; i < initialPoolSize; i++)
            {
                PooledConnections.Enqueue((new Connection(database)));
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
                foreach(var pooledConnection in PooledConnections)
                {
                    pooledConnection.Dispose();
                }
                PooledConnections.Clear();
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Free a connection. Do not use the connection after calling this method.
        /// </summary>
        /// <param name="connection"></param>
        public void FreeConnection(Connection connection)
        {
            if(connection == null)
                return;

            lock(locker)
            {
                PooledConnections.Enqueue(connection);
            }
        }

        /// <summary>
        /// Take a connection from the pool.
        /// </summary>
        /// <returns></returns>
        public Connection TakeConnection()
        {
            lock(locker)
            {
                return PooledConnections.Any() ? PooledConnections.Dequeue() : new Connection(database);
            }
        }

        /// <summary>
        /// Perform an action (<paramref name="actionWithConnection"/>) in the context of a <see cref="Connection"/>.
        /// </summary>
        /// <param name="actionWithConnection"></param>
        public void WithConnection(Action<Connection> actionWithConnection)
        {
            var connection = TakeConnection();
            try
            {
                actionWithConnection(connection);
            }
            finally
            {
                FreeConnection(connection);
            }
        }

        /// <summary>
        /// Get the result <typeparamref name="T"/> of a function (<paramref name="funcWithConnection"/>
        /// in the context of a <see cref="Connection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWithConnection"></param>
        /// <returns></returns>
        public T WithConnection<T>(Func<Connection, T> funcWithConnection)
        {
            var connection = TakeConnection();
            try
            {
                return funcWithConnection(connection);
            }
            finally
            {
                FreeConnection(connection);
            }
        }

        ~RealConnectionPool()
        {
            Dispose();
        }
    }
}