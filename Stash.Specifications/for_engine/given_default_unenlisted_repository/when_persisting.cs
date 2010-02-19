namespace Stash.Specifications.for_engine.given_default_unenlisted_repository
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;
    using Is = Rhino.Mocks.Constraints.Is;

    [TestFixture]
    public class when_persisting
    {
        [Test]
        public void it_should_make_the_event_via_the_persistence_event_factory()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockPersistenceEventFactory = MockRepository.GenerateMock<PersistenceEventFactory>();
            var sut = new DefaultUnenlistedRepository();
            var graph = new DummyPersistentObject();
            var mockEndure = MockRepository.GenerateStub<Endure<DummyPersistentObject>>(null, null);

            mockSession.Expect(s => s.Registry).Return(mockRegistry);
            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(null)).IgnoreArguments().Return(true);
            mockSession.Expect(_ => _.PersistenceEventFactory).Return(mockPersistenceEventFactory);
            mockPersistenceEventFactory.Expect(_ => _.MakeEndure<DummyPersistentObject>(null, null)).IgnoreArguments().Return(mockEndure);

            sut.Persist(mockSession, graph);

            mockPersistenceEventFactory.VerifyAllExpectations();
        }

        [Test]
        public void it_should_ensure_that_the_registry_is_managing_the_graph_type()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var sut = new DefaultUnenlistedRepository();
            var graph = new DummyPersistentObject();

            mockSession.Expect(s => s.Registry).Return(mockRegistry);
            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(typeof(DummyPersistentObject))).Return(false);

            typeof(ArgumentOutOfRangeException).ShouldBeThrownBy(
                () => sut.Persist(mockSession, graph));
        }

    }
}