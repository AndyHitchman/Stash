namespace Stash.Specifications.for_configuration.given_persistence_context
{
    using System;
    using Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_persist_a_graph : with_configure_stash
    {
        [Test]
        public void it_should_insist_that_the_graph_is_not_already_registered()
        {
            Sut.For<DummyPersistentObject>();
            typeof(ArgumentException)
                .ShouldBeThrownBy(
                () =>
                Sut.For<DummyPersistentObject>());
        }

        [Test]
        public void it_should_provide_a_graph_context_for_additional_configuration()
        {
            GraphContext<DummyPersistentObject> actual = null;
            Sut.For<DummyPersistentObject>(context => { actual = context; });
            actual.ShouldNotBeNull();
        }

        [Test]
        public void it_should_provide_a_graph_context_with_a_registered_graph()
        {
            GraphContext<DummyPersistentObject> actual = null;
            Sut.For<DummyPersistentObject>(context => { actual = context; });
            actual.RegisteredGraph.ShouldBeOfType(typeof(RegisteredGraph<DummyPersistentObject>));
        }

        [Test]
        public void it_should_provide_a_graph_context_with_a_registered_graph_having_an_assigned_aggregate_type()
        {
            GraphContext<DummyPersistentObject> actual = null;
            Sut.For<DummyPersistentObject>(context => { actual = context; });
            actual.RegisteredGraph.AggregateType.ShouldEqual(typeof(DummyPersistentObject));
        }

        [Test]
        public void it_should_register_a_graph()
        {
            Sut.For<DummyPersistentObject>();
            Sut.RegisteredGraphs.ShouldHaveCount(1);
        }

        [Test]
        public void it_should_register_the_provided_distinct_graphs()
        {
            Sut.For<DummyPersistentObject>();
            Sut.For<OtherDummyPersistentObject>();
            Sut.RegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<DummyPersistentObject>));
            Sut.RegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<OtherDummyPersistentObject>));
        }

        [Test]
        public void it_should_register_the_provided_graph()
        {
            Sut.For<DummyPersistentObject>();
            Sut.RegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<DummyPersistentObject>));
        }
    }
}