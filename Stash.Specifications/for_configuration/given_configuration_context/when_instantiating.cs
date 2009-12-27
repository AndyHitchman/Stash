namespace Stash.Specifications.for_configuration.given_configuration_context
{
    using NUnit.Framework;

    [TestFixture]
    public class when_instantiating : with_configure_stash
    {
        [Test]
        public void it_should_have_no_configured_graphs()
        {
            Sut.RegisteredGraphs.ShouldBeEmpty();
        }
    }
}