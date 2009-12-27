namespace Stash.Specifications.for_configuration.given_configuration_engine
{
    using Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class when_configuring
    {
        [Test]
        public void it_should_provide_a_configuration_context_to_scope_configuration()
        {
            PersistenceContext actual = null;
            var sut = new ConfigurationEngine<DummyBackingStore>();
            sut.Persist(context => { actual = context; });
            actual.ShouldNotBeNull();
        }

    }
}