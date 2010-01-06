namespace Stash.Specifications.for_configuration.given_registered_stash
{
    using NUnit.Framework;

    [TestFixture]
    public class when_instantiating : with_registered_stash
    {
        [Test]
        public void it_should_have_no_configured_graphs()
        {
            Sut.AllRegisteredGraphs.ShouldBeEmpty();
        }
    }
}