namespace Stash.Specifications.for_configuration.given_configure_stash
{
    using Configuration;
    using NUnit.Framework;

    public class with_configure_stash
    {
        protected DummyConfiguration SutProxy { get; private set; }

        [SetUp]
        public void each_up()
        {
            SutProxy = new DummyConfiguration();
        }
    }
}