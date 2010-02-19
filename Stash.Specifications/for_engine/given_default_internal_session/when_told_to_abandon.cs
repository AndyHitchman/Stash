namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using System.Collections.Generic;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    [TestFixture]
    public class when_told_to_abandon
    {
        [Test]
        public void it_should_clear_all_peristed_events()
        {
            var sut = new StandInDefaultInternalSession();
            var mockPersistentEvent = MockRepository.GenerateStub<PersistenceEvent>();
            sut.ExposedPersistenceEvents.Add(mockPersistentEvent);

            sut.Abandon();

            sut.EnrolledPersistenceEvents.ShouldBeEmpty();
        }
    }
}