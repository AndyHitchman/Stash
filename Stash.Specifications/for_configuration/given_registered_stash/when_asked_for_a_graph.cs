namespace Stash.Specifications.for_configuration.given_registered_stash
{
    using System;
    using Configuration;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_asked_for_a_graph : with_registered_stash
    {
        [Test]
        public void it_should_complain_if_the_type_is_not_a_registered_graph()
        {
            typeof(ArgumentOutOfRangeException)
                .ShouldBeThrownBy(
                () =>
                Sut.GetRegistrationFor(typeof(DummyPersistentObject)));
        }

        [Test]
        public void it_should_complain_if_the_type_is_null()
        {
            typeof(ArgumentNullException)
                .ShouldBeThrownBy(
                () =>
                Sut.GetRegistrationFor(null));
        }

        [Test]
        public void it_should_get_the_graph_for_a_registered_graph_by_generic_wrapper()
        {
            Sut.RegisteredGraphs.Add(typeof(DummyPersistentObject), new RegisteredGraph<DummyPersistentObject>());

            Sut.GetRegistrationFor<DummyPersistentObject>().ShouldNotBeNull();
        }

        [Test]
        public void it_should_get_the_graph_for_a_registered_graph_by_type()
        {
            Sut.RegisteredGraphs.Add(typeof(DummyPersistentObject), new RegisteredGraph<DummyPersistentObject>());

            Sut.GetRegistrationFor(typeof(DummyPersistentObject)).ShouldNotBeNull();
        }
    }
}