namespace Stash.Specifications.for_engine.given_default_enlisted_repository
{
    using Engine;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    [TestFixture]
    public class when_initialising
    {
        [Test]
        public void it_should_operate_in_the_context_of_the_enlisting_session()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.EnlistedSession.ShouldEqual(mockSession);
        }
    }
}