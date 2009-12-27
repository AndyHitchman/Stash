namespace Stash.Specifications.for_configuration.given_configure_stash
{
    using Configuration;
    using NUnit.Framework;

    public class with_configure_stash
    {
        protected ConfigurationContext Sut { get; private set; }

        [SetUp]
        public void each_up()
        {
            Sut = new ConfigurationContext();
        }
    }
}