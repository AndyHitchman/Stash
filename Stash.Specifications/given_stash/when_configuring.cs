namespace Stash.Specifications.given_stash
{
    using Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class when_configuring
    {
        [Test]
        public void it_should_provide_a_configuration_context_to_scope_configuration()
        {
            ConfigurationContext actual = null;
            Stash.Configure(context => { actual = context; });
            actual.ShouldNotBeNull();
        }

    }
}