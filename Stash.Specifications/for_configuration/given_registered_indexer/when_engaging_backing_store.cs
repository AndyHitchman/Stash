namespace Stash.Specifications.for_configuration.given_registered_indexer
{
    using Configuration;
    using Engine;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_engaging_backing_store
    {
        [Test]
        public void it_should_tell_the_backing_store_to_ensure_the_indexer_is_managed()
        {
            var mockBackingStore = MockRepository.GenerateMock<BackingStore>();
            var fakeIndexer = MockRepository.GenerateStub<Indexer<DummyPersistentObject, object>>();
            var sut = new RegisteredIndexer<DummyPersistentObject, object>(fakeIndexer);

            sut.EngageBackingStore(mockBackingStore);

            mockBackingStore.AssertWasCalled(backingStore => backingStore.EnsureIndexer(fakeIndexer));
        }
    }
}