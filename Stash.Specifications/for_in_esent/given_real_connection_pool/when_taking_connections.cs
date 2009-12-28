namespace Stash.Specifications.for_in_esent.given_real_connection_pool
{
    using System.Linq;
    using In.ESENT;
    using NUnit.Framework;

    [TestFixture]
    public class when_taking_connections
    {
        [Test]
        public void it_should_take_them_from_the_pool()
        {
            ushort actual = 1;
            var expected = actual - 1;
            var sut = new RealConnectionPool(null, actual);

            sut.TakeConnection();
            
            sut.PooledConnections.Count.ShouldEqual(expected);
        }

        [Test]
        public void it_should_create_them_dynamically_if_the_pool_is_zero_sized()
        {
            var sut = new RealConnectionPool(null, 0);

            sut.TakeConnection().ShouldBeOfType<Connection>();
        }

        [Test]
        public void it_should_create_them_dynamically_if_the_pool_is_empty()
        {
            var sut = new RealConnectionPool(null, 1);
            sut.TakeConnection();

            sut.PooledConnections.Any().ShouldBeFalse();
            sut.TakeConnection().ShouldBeOfType<Connection>();
        }

    }
}