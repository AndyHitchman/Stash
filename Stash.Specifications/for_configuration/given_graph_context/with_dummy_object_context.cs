namespace Stash.Specifications.for_configuration.given_graph_context
{
    using Configuration;
    using NUnit.Framework;

    public class with_dummy_object_context
    {
        protected GraphContext<DummyPersistentObject> Sut { get; private set; }

        [SetUp]
        public void each_up()
        {
            Sut = new GraphContext<DummyPersistentObject>(new RegisteredGraph<DummyPersistentObject>());
        }
    }
}