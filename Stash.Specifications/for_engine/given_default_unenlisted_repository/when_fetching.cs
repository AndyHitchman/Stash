namespace Stash.Specifications.for_engine.given_default_unenlisted_repository
{
    using System;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Selectors;
    using Is = Rhino.Mocks.Constraints.Is;

    [TestFixture]
    public class when_fetching
    {
        //TODO : Refactor repository to simplify tests.

        [Test]
        public void it_should_give_the_session_a_persistence_track_for_each_graph()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSelector =
                MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((Projector<object, DummyPersistentObject>)null);
            var mockRegisteredGraph = MockRepository.GenerateMock<RegisteredGraph<DummyPersistentObject>>();
            var sut = new DefaultUnenlistedRepository();

            mockSession.Expect(s => s.Registry).Return(mockRegistry);
            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(null)).IgnoreArguments().Return(true);
            mockRegistry.Expect(r => r.GetRegistrationFor<DummyPersistentObject>()).Return(mockRegisteredGraph);

            sut.Fetch(mockSession, new[] { mockSelector });

            mockSession.AssertWasCalled(
                session => session.Enroll(null),
                o => o.Constraints(Is.TypeOf<Track<DummyPersistentObject>>()));
        }

        [Test]
        public void it_should_give_the_session_a_persistence_track_for_each_projection()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSelector =
                MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((Projector<object, DummyPersistentObject>)null);
            var sut = new DefaultUnenlistedRepository();

            mockSession.Expect(s => s.Registry).Return(mockRegistry);
            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(null)).IgnoreArguments().Return(true);
            
            sut.Fetch(mockSession, mockSelector);

            mockSession.AssertWasCalled(
                session => session.Enroll(null),
                o => o.Constraints(Is.TypeOf<Track<DummyPersistentObject>>()));
        }

        [Test, Ignore("Isn't valid: Can fetch maps and reductions")]
        public void it_should_ensure_that_the_registry_is_managing_the_graph_type_for_a_projected_fetch()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSelector =
                MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((Projector<object, DummyPersistentObject>)null);
            var sut = new DefaultUnenlistedRepository();
            var graph = new DummyPersistentObject();

            mockSession.Expect(s => s.Registry).Return(mockRegistry);
            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(typeof(DummyPersistentObject))).Return(false);

            typeof(ArgumentOutOfRangeException).ShouldBeThrownBy(
                () => sut.Fetch(mockSession, mockSelector));
        }

        [Test,Ignore("Isn't valid: Can fetch maps and reductions")]
        public void it_should_ensure_that_the_registry_is_managing_the_graph_type_for_a_graph_fetch()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var mockSelector =
                MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((Projector<object, DummyPersistentObject>)null);
            var sut = new DefaultUnenlistedRepository();
            var graph = new DummyPersistentObject();

            mockSession.Expect(s => s.Registry).Return(mockRegistry);
            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(typeof(DummyPersistentObject))).Return(false);

            typeof(ArgumentOutOfRangeException).ShouldBeThrownBy(
                () => sut.Fetch(mockSession, new[] {mockSelector}));
        }
    }
}