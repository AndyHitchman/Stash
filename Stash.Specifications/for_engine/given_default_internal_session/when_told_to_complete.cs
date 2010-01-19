namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using Configuration;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_told_to_complete
    {
        [Test]
        public void it_should_clear_all_peristed_events()
        {
            var sut = new StandInDefaultInternalSession();
            var mockPersistentEvent = MockRepository.GenerateStub<PersistenceEvent>();
            sut.ExposedPersistenceEvents.Add(mockPersistentEvent);

            sut.Complete();

            sut.EnrolledPersistenceEvents.ShouldBeEmpty();
        }

        [Test]
        public void it_should_tell_persisted_events_to_complete()
        {
            var sut = new StandInDefaultInternalSession();
            var mockPersistentEvent = MockRepository.GenerateMock<PersistenceEvent>();
            sut.ExposedPersistenceEvents.Add(mockPersistentEvent);

            sut.Complete();

            mockPersistentEvent.AssertWasCalled(_ => _.Complete());
        }
    }
}