namespace Stash.Specifications
{
    using System;
    using In.ESENT;

    public class DummyConnectionPool : ConnectionPool
    {
        public void Dispose()
        {
        }

        public void FreeConnection(Connection connection)
        {
            throw new NotImplementedException();
        }

        public Connection TakeConnection()
        {
            throw new NotImplementedException();
        }

        public void WithConnection(Action<Connection> actionWithConnection)
        {
            throw new NotImplementedException();
        }

        public T WithConnection<T>(Func<Connection, T> funcWithConnection)
        {
            throw new NotImplementedException();
        }
    }
}