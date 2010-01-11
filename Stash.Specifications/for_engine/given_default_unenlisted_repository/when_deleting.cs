namespace Stash.Specifications.for_engine.given_default_unenlisted_repository
{
    using System.Linq.Expressions;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_deleting
    {
        [Test]
        public void it_should_give_the_session_a_persistence_event()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var sut = new DefaultUnenlistedRepository();
            var graph = new DummyPersistentObject();

            sut.Delete(mockSession, graph);

            mockSession.AssertWasCalled(
                session => session.Enroll<DummyPersistentObject>(null),
                o => o.Constraints(Rhino.Mocks.Constraints.Is.TypeOf<Destroy<DummyPersistentObject>>()));
        }
    }
}