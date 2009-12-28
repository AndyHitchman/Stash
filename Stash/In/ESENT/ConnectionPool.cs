namespace Stash.In.ESENT
{
    using System;

    public interface ConnectionPool : IDisposable
    {
        /// <summary>
        /// Take a connection from the pool.
        /// </summary>
        /// <returns></returns>
        Connection TakeConnection();

        /// <summary>
        /// Free a connection. Do not use the connection after calling this method.
        /// </summary>
        /// <param name="connection"></param>
        void FreeConnection(Connection connection);

        /// <summary>
        /// Perform an action (<paramref name="actionWithConnection"/>) in the context of a <see cref="Connection"/>.
        /// </summary>
        /// <param name="actionWithConnection"></param>
        void WithConnection(Action<Connection> actionWithConnection);

        /// <summary>
        /// Get the result <typeparamref name="T"/> of a function (<paramref name="funcWithConnection"/>
        /// in the context of a <see cref="Connection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWithConnection"></param>
        /// <returns></returns>
        T WithConnection<T>(Func<Connection,T> funcWithConnection);
    }
}