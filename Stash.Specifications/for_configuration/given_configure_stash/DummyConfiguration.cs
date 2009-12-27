namespace Stash.Specifications.for_configuration.given_configure_stash
{
    using System;
    using Configuration;

    public class DummyConfiguration : Configuration.ConfigureStash
    {
        public new void For<TGraph>(Action<GraphContext<TGraph>> configurePersistentGraph)
        {
            base.For(configurePersistentGraph);
        }

        public new void For<TGraph>()
        {
            base.For<TGraph>();
        }
    }
}