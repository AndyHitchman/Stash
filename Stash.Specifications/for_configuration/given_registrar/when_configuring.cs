namespace Stash.Specifications.for_configuration.given_registrar
{
    using Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class when_configuring
    {
        [Test]
        public void it_should_provide_a_configuration_context_to_scope_configuration()
        {
            PersistenceContext<DummyBackingStore> actual = null;
            var sut = new Registrar<DummyBackingStore>(new DummyBackingStore());
            sut.PerformRegistration(context => { actual = context; });
            actual.ShouldNotBeNull();
        }
    }
}