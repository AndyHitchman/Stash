namespace Stash.Specifications.for_in_esent.given_real_connection_pool
{
    using In.ESENT;
    using NUnit.Framework;

    [TestFixture]
    public class when_contructing
    {
        [Test]
        public void it_should_pool_connections()
        {
            var sut = new RealConnectionPool(null, 5);
            sut.PooledConnections.Count.ShouldEqual(5);
        }

        [Test]
        public void it_should_not_pool_connections_when_given_initial_size_of_zero()
        {
            var sut = new RealConnectionPool(null, 0);
            sut.PooledConnections.Count.ShouldEqual(0);
        }
    }
}