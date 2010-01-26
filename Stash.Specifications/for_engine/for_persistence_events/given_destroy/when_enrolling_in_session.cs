namespace Stash.Specifications.for_engine.for_persistence_events.given_destroy
{
    using System;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_enrolling_in_session
    {
        [Test]
        public void it_should_tell_the_session_about_itself()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var graph = new DummyPersistentObject();
            var sut = new Destroy<DummyPersistentObject>(Guid.Empty, null, mockSession);

            sut.EnrollInSession();

            mockSession.AssertWasCalled(
                session => session.Enroll(sut));

        }
    }
}