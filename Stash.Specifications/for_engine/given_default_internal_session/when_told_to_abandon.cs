namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using System.Collections.Generic;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_told_to_abandon
    {
        [Test]
        public void it_should_clear_all_peristed_events()
        {
            var sut = new DefaultInternalSession(null);
            var mockPersistentEvent = MockRepository.GenerateStub<PersistenceEvent>();
            ((List<PersistenceEvent>)sut.EnrolledPersistenceEvents).Add(mockPersistentEvent);

            sut.Abandon();

            sut.EnrolledPersistenceEvents.ShouldBeEmpty();
        }
    }
}