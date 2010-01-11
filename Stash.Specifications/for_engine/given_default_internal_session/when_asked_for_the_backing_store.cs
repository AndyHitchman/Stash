namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using Configuration;
    using Engine;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_asked_for_the_backing_store
    {
        [Test]
        public void it_should_return_the_backing_store_given_to_the_registration()
        {
            var mockRegistration = MockRepository.GenerateMock<Registration>();
            
            var sut = new DefaultInternalSession(mockRegistration);
            var actual = sut.BackingStore;

            mockRegistration.AssertWasCalled(registration => { var x = registration.BackingStore; });
        }
    }
}