namespace Stash.Specifications.for_configuration.given_registered_stash
{
    using Configuration;
    using Engine;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_engaging_backing_store
    {
        [Test]
        public void it_should_tell_registered_graphs_to_engage_themselves()
        {
            var mockRegisteredGraph = MockRepository.GenerateMock<RegisteredGraph<DummyPersistentObject>>();
            var fakeBackingStore = MockRepository.GenerateStub<BackingStore>();
            var sut = new Registration();
            sut.RegisteredGraphs.Add(typeof(DummyPersistentObject), mockRegisteredGraph);

            sut.EngageBackingStore(fakeBackingStore);

            mockRegisteredGraph.AssertWasCalled(graph => graph.EngageBackingStore(fakeBackingStore));
        }
    }
}