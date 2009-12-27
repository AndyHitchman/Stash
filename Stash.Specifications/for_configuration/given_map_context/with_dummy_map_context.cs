namespace Stash.Specifications.for_configuration.given_map_context
{
    using Configuration;
    using NUnit.Framework;

    public class with_dummy_map_context
    {
        protected MapContext<DummyPersistentObject> Sut { get; private set; }

        [SetUp]
        public void each_up()
        {
            Sut = new MapContext<DummyPersistentObject>();
        }
    }
}