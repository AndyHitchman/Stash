namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using System.Collections.Generic;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    [TestFixture]
    public class when_told_to_enroll
    {
        [Test]
        public void it_should_record_the_peristed_event()
        {
            var sut = new DefaultInternalSession(null, null);
            var mockPersistentEvent = MockRepository.GenerateStub<PersistenceEvent>();

            sut.Enroll(mockPersistentEvent);

            sut.EnrolledPersistenceEvents.ShouldContain(mockPersistentEvent);
        }
    }
}