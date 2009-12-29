namespace Stash.Specifications.for_configuration.given_graph_context
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_map : with_dummy_object_context
    {
        [Test]
        public void it_should_complain_if_the_mapper_is_null()
        {
            Mapper<DummyPersistentObject> expected = null;

            typeof(ArgumentNullException)
                .ShouldBeThrownBy(() => Sut.MapWith(expected));
        }

        [Test]
        public void it_should_register_the_mapper()
        {
            var expected = new DummyMapper();
            Sut.MapWith(expected);
            Sut.RegisteredGraph.RegisteredMappers.ShouldContain(mapper => mapper.Mapper == expected);
        }

        [Test]
        public void it_should_provide_a_map_context_for_further_configuration()
        {
            var expected = new DummyMapper();
            var actual = Sut.MapWith(expected);
            actual.ShouldNotBeNull();
            actual.RegisteredMapper.ShouldNotBeNull();
        }
    }
}