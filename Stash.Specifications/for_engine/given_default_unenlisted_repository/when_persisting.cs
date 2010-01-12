namespace Stash.Specifications.for_engine.given_default_unenlisted_repository
{
    using System;
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
            var sut = new DefaultUnenlistedRepository();
            var graph = new DummyPersistentObject();

            mockSession.Expect(s => s.Registry).Return(mockRegistry).Repeat.Any();
            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(null)).IgnoreArguments().Return(true);

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