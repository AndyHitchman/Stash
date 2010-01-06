namespace Stash.Specifications.for_in_esent.given_real_connection_pool
{
    using In.ESENT;
    using NUnit.Framework;

    [TestFixture][Ignore]
    public class when_freeing_connections : with_dummy_instance
    {
        [Test]
        public void it_should_add_them_to_the_pool()
        {
            var connection = new Connection(new Database(Instance, null));
            var sut = new RealConnectionPool(new Database(Instance, null), 0);

            sut.FreeConnection(connection);

            sut.PooledConnections.Count.ShouldEqual(1);
        }
    }
}