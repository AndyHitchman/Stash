namespace Stash.Specifications.for_configuration.given_configure_stash
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_persist_a_graph : with_configure_stash
    {
        [Test]
        public void it_should_throw_nix()
        {
            typeof(NotImplementedException)
                .ShouldBeThrownBy(() =>
                                  SutProxy.For<DummyPersistentObject>(context => { }));
        }

        [Test]
        public void it_should_register_a_graph()
        {
            SutProxy.For<DummyPersistentObject>();
            SutProxy.RegisteredGraphs.ShouldHaveCount(1);
        }

        [Test]
        public void it_should_register_the_provided_graph()
        {
            SutProxy.For<DummyPersistentObject>();
            SutProxy.RegisteredGraphs.ShouldContain(graph => graph.AggregateType == typeof (DummyPersistentObject));
        }

        [Test]
        public void it_should_insist_that_the_graph_is_not_already_registered()
        {
            SutProxy.For<DummyPersistentObject>();
            typeof(ArgumentException)
                .ShouldBeThrownBy(() =>
                                  SutProxy.For<DummyPersistentObject>());
        }
    }
}