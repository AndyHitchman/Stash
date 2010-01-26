namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using Configuration;
    using Engine;
    using NUnit.Framework;

    [TestFixture]
    public class when_asked_for_the_registry
    {
        [Test]
        public void it_should_return_the_provided_registration()
        {
            var fakeRegistry = new Registry();

            var sut = new DefaultInternalSession(fakeRegistry, null);

            sut.Registry.ShouldEqual(fakeRegistry);
        }
    }
}