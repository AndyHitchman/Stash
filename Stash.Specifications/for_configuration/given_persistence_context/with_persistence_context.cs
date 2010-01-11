namespace Stash.Specifications.for_configuration.given_persistence_context
{
    using Configuration;
    using NUnit.Framework;

    public class with_persistence_context
    {
        protected PersistenceContext<DummyBackingStore> Sut { get; private set; }

        [SetUp]
        public void each_up()
        {
            Sut = new PersistenceContext<DummyBackingStore>(new Registry());
        }
    }
}