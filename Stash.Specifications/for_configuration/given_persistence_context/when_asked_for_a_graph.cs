namespace Stash.Specifications.for_configuration.given_persistence_context
{
    using System;
    using Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class when_asked_for_a_graph : with_configure_stash
    {
        [Test]
        public void it_should_complain_if_the_type_is_null()
        {
            typeof(ArgumentNullException)
                .ShouldBeThrownBy(
                () =>
                Sut.GetGraphFor(null));
        }

        [Test]
        public void it_should_complain_if_the_type_is_not_a_registered_graph()
        {
            typeof(ArgumentOutOfRangeException)
                .ShouldBeThrownBy(
                () =>
                Sut.GetGraphFor(typeof(DummyPersistentObject)));
        }

        [Test]
        public void it_should_get_the_graph_for_a_registered_graph_by_type()
        {
            Sut.Register<DummyPersistentObject>();

            Sut.GetGraphFor(typeof(DummyPersistentObject)).ShouldNotBeNull();
        }

        [Test]
        public void it_should_get_the_graph_for_a_registered_graph_by_generic_wrapper()
        {
            Sut.Register<DummyPersistentObject>();

            Sut.GetGraphFor<DummyPersistentObject>().ShouldNotBeNull();
        }
   }
}