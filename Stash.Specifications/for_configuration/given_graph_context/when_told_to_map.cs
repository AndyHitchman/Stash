namespace Stash.Specifications.for_configuration.given_graph_context
{
    using System;
    using Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_map : with_dummy_graph_context
    {
        [Test]
        public void it_should_complain_if_the_mapper_is_null()
        {
            Map<DummyPersistentObject,object,object> expected = null;

            typeof(ArgumentNullException)
                .ShouldBeThrownBy(() => Sut.MapWith(expected));
        }

        [Test]
        public void it_should_provide_a_map_context_for_further_configuration()
        {
            var expected = new DummyMap();
            var actual = Sut.MapWith(expected);
            actual.ShouldNotBeNull();
            actual.RegisteredMapper.ShouldNotBeNull();
        }

        [Test]
        public void it_should_register_the_mapper()
        {
            var expected = new DummyMap();
            Sut.MapWith(expected);
            Sut.RegisteredGraph.RegisteredMappers.ShouldContain(mapper => ((RegisteredMapper<DummyPersistentObject,object,object>)mapper).Map == expected);
        }
    }
}