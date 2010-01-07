namespace Stash.Specifications.for_engine.given_actual_unenlisted_repository
{
    using System.Linq.Expressions;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_persisting
    {
        [Test]
        public void it_should_give_the_session_a_persistence_event()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var sut = new ActualUnenlistedRepository();
            var graph = new DummyPersistentObject();

            sut.Persist(mockSession, graph);

            mockSession.AssertWasCalled(
                session => session.Enroll<DummyPersistentObject>(null),
                o => o.Constraints(Rhino.Mocks.Constraints.Is.TypeOf<Endure<DummyPersistentObject>>()));
        }
    }
}