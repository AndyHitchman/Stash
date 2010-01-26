namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using Engine;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_enlist_repository
    {
        [Test]
        public void it_should_return_an_enlisted_repository()
        {
            var sut = new DefaultInternalSession(null, null);

            sut.EnlistRepository(null).ShouldNotBeNull();
        }
    }
}