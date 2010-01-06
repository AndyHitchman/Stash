namespace Stash.Specifications.for_configuration.given_registered_stash
{
    using System;
    using Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_register_a_graph : with_registered_stash
    {
        [Test]
        public void it_should_insist_that_the_graph_is_not_already_registered()
        {
            Sut.RegisterGraph<DummyPersistentObject>();
            typeof(ArgumentException)
                .ShouldBeThrownBy(
                () =>
                Sut.RegisterGraph<DummyPersistentObject>());
        }

        [Test]
        public void it_should_register_a_graph()
        {
            Sut.RegisterGraph<DummyPersistentObject>();
            Sut.AllRegisteredGraphs.ShouldHaveCount(1);
        }

        [Test]
        public void it_should_register_the_provided_distinct_graphs()
        {
            Sut.RegisterGraph<DummyPersistentObject>();
            Sut.RegisterGraph<OtherDummyPersistentObject>();
            Sut.AllRegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<DummyPersistentObject>));
            Sut.AllRegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<OtherDummyPersistentObject>));
        }

        [Test]
        public void it_should_register_the_provided_graph()
        {
            Sut.RegisterGraph<DummyPersistentObject>();
            Sut.AllRegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<DummyPersistentObject>));
        }
    }
}