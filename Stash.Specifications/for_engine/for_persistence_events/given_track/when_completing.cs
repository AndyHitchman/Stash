namespace Stash.Specifications.for_engine.for_persistence_events.given_track
{
    using System;
    using System.IO;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using Engine.Serializers;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Is = Rhino.Mocks.Constraints.Is;

    [TestFixture]
    public class when_completing
    {
        [Test]
        public void it_should_enroll_an_update_persistence_event_when_the_graph_has_changed()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSerializer = MockRepository.GenerateMock<Serializer>();
            Func<Serializer> fSerializer = () => mockSerializer;
            var graph = new DummyPersistentObject();
            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, new MemoryStream(), mockSession);

            mockSession.Expect(_ => _.Registry).Return(mockRegistry);
            mockRegistry.Expect(_ => _.Serializer).Return(fSerializer);
            mockSerializer.Expect(_ => _.Serialize(null)).IgnoreArguments().Return(new MemoryStream(new byte[] {1}));

            sut.Complete();

            mockSession.AssertWasCalled(s => s.Enroll(null), o => o.Constraints(Is.TypeOf<Update<DummyPersistentObject>>()));
        }

        [Test]
        public void it_should_not_enroll_an_update_persistence_event_when_the_graph_has_not_changed()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSerializer = MockRepository.GenerateMock<Serializer>();
            Func<Serializer> fSerializer = () => mockSerializer;
            var graph = new DummyPersistentObject();
            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, new MemoryStream(), mockSession);

            mockSession.Expect(_ => _.Registry).Return(mockRegistry);
            mockRegistry.Expect(_ => _.Serializer).Return(fSerializer);
            mockSerializer.Expect(_ => _.Serialize(null)).IgnoreArguments().Return(new MemoryStream());

            sut.Complete();

            mockSession.AssertWasNotCalled(s => s.Enroll(null), o => o.IgnoreArguments());
        }
    }
}