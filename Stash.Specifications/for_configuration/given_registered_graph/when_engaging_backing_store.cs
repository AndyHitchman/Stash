namespace Stash.Specifications.for_configuration.given_registered_graph
{
    using Configuration;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_engaging_backing_store
    {
        [Test]
        public void it_should_tell_registered_indexers_to_engage_themselves()
        {
            var mockRegisteredIndexer =
                MockRepository.GenerateMock<RegisteredIndexer<DummyPersistentObject, object>>((Index<DummyPersistentObject, object>)null);
            var sut = new RegisteredGraph<DummyPersistentObject>();
            sut.RegisteredIndexers.Add(mockRegisteredIndexer);

            sut.EngageBackingStore(null);

            mockRegisteredIndexer.AssertWasCalled(indexer => indexer.EngageBackingStore(null), options => options.IgnoreArguments());
        }
    }
}