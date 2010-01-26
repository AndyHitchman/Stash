namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using Engine;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_internalise
    {
        [Test]
        public void it_should_return_itself()
        {
            var sut = new DefaultInternalSession(null, null);

            sut.Internalize().ShouldEqual(sut);
        }
    }
}