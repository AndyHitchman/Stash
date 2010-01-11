namespace Stash.Specifications.for_engine.given_default_unenlisted_repository
{
    using System.Linq.Expressions;
    using Configuration;
    using Selectors;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_fetching
    {
        [Test]
        public void it_should_give_the_session_a_persistence_track_for_each_projection()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockSelector = MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((Projector<object,DummyPersistentObject>)null);
            var sut = new DefaultUnenlistedRepository();
            
            sut.Fetch(mockSession, mockSelector);

            mockSession.AssertWasCalled(
                session => session.Enroll<DummyPersistentObject>(null),
                o => o.Constraints(Rhino.Mocks.Constraints.Is.TypeOf<Track<DummyPersistentObject>>()));
        }

        [Test]
        public void it_should_give_the_session_a_persistence_track_for_each_graph()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockSelector = MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((Projector<object,DummyPersistentObject>)null);
            var sut = new DefaultUnenlistedRepository();
            
            sut.Fetch(mockSession, new[] {mockSelector});

            mockSession.AssertWasCalled(
                session => session.Enroll<DummyPersistentObject>(null),
                o => o.Constraints(Rhino.Mocks.Constraints.Is.TypeOf<Track<DummyPersistentObject>>()));
        }
    }
}