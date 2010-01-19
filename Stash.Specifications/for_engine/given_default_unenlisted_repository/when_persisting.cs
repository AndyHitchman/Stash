namespace Stash.Specifications.for_engine.given_default_unenlisted_repository
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Is = Rhino.Mocks.Constraints.Is;

    [TestFixture]
    public class when_persisting
    {
        [Test]
        public void it_should_give_the_session_a_persistence_event()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockRegisteredGraph = MockRepository.GenerateMock<RegisteredGraph<DummyPersistentObject>>();
            var sut = new DefaultUnenlistedRepository();
            var graph = new DummyPersistentObject();

            mockSession.Expect(_ => _.Registry).Return(mockRegistry).Repeat.Any();
            mockRegistry.Expect(_ => _.IsManagingGraphTypeOrAncestor(null)).IgnoreArguments().Return(true);
            mockRegistry.Expect(_ => _.GetRegistrationFor<DummyPersistentObject>()).Return(mockRegisteredGraph);
            mockRegisteredGraph.Expect(_ => _.RegisteredIndexers).Return(new RegisteredIndexer<DummyPersistentObject>[] {});
            mockRegisteredGraph.Expect(_ => _.RegisteredMappers).Return(new RegisteredMapper<DummyPersistentObject>[] {});

            sut.Persist(mockSession, graph);

            mockSession.AssertWasCalled(
                session => session.Enroll(null),
                o => o.Constraints(Is.TypeOf<Endure<DummyPersistentObject>>()));
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