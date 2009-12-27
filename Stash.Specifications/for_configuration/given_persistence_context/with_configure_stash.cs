namespace Stash.Specifications.for_configuration.given_persistence_context
{
    using Configuration;
    using NUnit.Framework;

    public class with_configure_stash
    {
        protected PersistenceContext Sut { get; private set; }

        [SetUp]
        public void each_up()
        {
            Sut = new PersistenceContext();
        }
    }
}