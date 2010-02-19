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
    using Support;

    [TestFixture]
    public class when_completing
    {
        private class StandInTrack<TGraph> : Track<TGraph>
        {
            public bool HasCalculatedIndexes;
            public bool HasCalculatedMaps;

            public StandInTrack(Guid internalId, TGraph graph, Stream serializedgraph, InternalSession session)
                : base(internalId, graph, serializedgraph, session)
            {
            }

            protected override void CalculateIndexes(RegisteredGraph<TGraph> registeredGraph)
            {
                HasCalculatedIndexes = true;
            }

            protected override void CalculateMaps(RegisteredGraph<TGraph> registeredGraph)
            {
                HasCalculatedMaps = true;
            }
        }

        [Test]
        public void it_should_calculate_indexes_when_the_graph_has_changed()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSerializer = MockRepository.GenerateMock<Serializer>();
            Func<Serializer> fSerializer = () => mockSerializer;
            var mockPersistenceEventFactory = MockRepository.GenerateMock<PersistenceEventFactory>();
            var graph = new DummyPersistentObject();
            var sut = new StandInTrack<DummyPersistentObject>(Guid.Empty, graph, Stream.Null, mockSession);
            var mockUpdate = new Update<DummyPersistentObject>(sut);

            mockSession.Expect(_ => _.Registry).Return(mockRegistry).Repeat.Any();
            mockSession.Expect(_ => _.PersistenceEventFactory).Return(mockPersistenceEventFactory);
            mockPersistenceEventFactory.Expect(_ => _.MakeUpdate(sut)).Return(mockUpdate);
            mockRegistry.Expect(_ => _.GetRegistrationFor<DummyPersistentObject>()).Return(null);
            mockRegistry.Expect(_ => _.Serializer).Return(fSerializer);
            mockSerializer.Expect(_ => _.Serialize(null)).IgnoreArguments().Return(new MemoryStream(new byte[] {1}));

            sut.Complete();

            sut.HasCalculatedIndexes.ShouldBeTrue();
        }

        [Test]
        public void it_should_calculate_maps_when_the_graph_has_changed()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSerializer = MockRepository.GenerateMock<Serializer>();
            Func<Serializer> fSerializer = () => mockSerializer;
            var mockPersistenceEventFactory = MockRepository.GenerateMock<PersistenceEventFactory>();
            var graph = new DummyPersistentObject();
            var sut = new StandInTrack<DummyPersistentObject>(Guid.Empty, graph, Stream.Null, mockSession);
            var mockUpdate = new Update<DummyPersistentObject>(sut);

            mockSession.Expect(_ => _.Registry).Return(mockRegistry).Repeat.Any();
            mockSession.Expect(_ => _.PersistenceEventFactory).Return(mockPersistenceEventFactory);
            mockPersistenceEventFactory.Expect(_ => _.MakeUpdate(sut)).Return(mockUpdate);
            mockRegistry.Expect(_ => _.GetRegistrationFor<DummyPersistentObject>()).Return(null);
            mockRegistry.Expect(_ => _.Serializer).Return(fSerializer);
            mockSerializer.Expect(_ => _.Serialize(null)).IgnoreArguments().Return(new MemoryStream(new byte[] {1}));

            sut.Complete();

            sut.HasCalculatedMaps.ShouldBeTrue();
        }

        [Test]
        public void it_should_enroll_an_update_persistence_event_when_the_graph_has_changed()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSerializer = MockRepository.GenerateMock<Serializer>();
            Func<Serializer> fSerializer = () => mockSerializer;
            var mockPersistenceEventFactory = MockRepository.GenerateMock<PersistenceEventFactory>();
            var graph = new DummyPersistentObject();
            var sut = new StandInTrack<DummyPersistentObject>(Guid.Empty, graph, Stream.Null, mockSession);
            var mockUpdate = new Update<DummyPersistentObject>(sut);

            mockSession.Expect(_ => _.Registry).Return(mockRegistry).Repeat.Any();
            mockSession.Expect(_ => _.PersistenceEventFactory).Return(mockPersistenceEventFactory);
            mockPersistenceEventFactory.Expect(_ => _.MakeUpdate(sut)).Return(mockUpdate);
            mockRegistry.Expect(_ => _.GetRegistrationFor<DummyPersistentObject>()).Return(null);
            mockRegistry.Expect(_ => _.Serializer).Return(fSerializer);
            mockSerializer.Expect(_ => _.Serialize(null)).IgnoreArguments().Return(new MemoryStream(new byte[] {1}));

            sut.Complete();

            mockSession.AssertWasCalled(s => s.Enroll(mockUpdate));
        }

        [Test]
        public void it_should_not_enroll_an_update_persistence_event_when_the_graph_has_not_changed()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSerializer = MockRepository.GenerateMock<Serializer>();
            Func<Serializer> fSerializer = () => mockSerializer;
            var graph = new DummyPersistentObject();
            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, Stream.Null, mockSession);

            mockSession.Expect(_ => _.Registry).Return(mockRegistry);
            mockRegistry.Expect(_ => _.Serializer).Return(fSerializer);
            mockSerializer.Expect(_ => _.Serialize(null)).IgnoreArguments().Return(Stream.Null);

            sut.Complete();

            mockSession.AssertWasNotCalled(s => s.Enroll(null), o => o.IgnoreArguments());
        }

        [Test]
        public void it_should_calculate_a_new_hash()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSerializer = MockRepository.GenerateMock<Serializer>();
            Func<Serializer> fSerializer = () => mockSerializer;
            var graph = new DummyPersistentObject();
            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, Stream.Null, mockSession);

            mockSession.Expect(_ => _.Registry).Return(mockRegistry);
            mockRegistry.Expect(_ => _.Serializer).Return(fSerializer);
            mockSerializer.Expect(_ => _.Serialize(null)).IgnoreArguments().Return(Stream.Null);

            sut.Complete();

            sut.CompletionHash.ShouldNotBeNull();
        }
    }
}