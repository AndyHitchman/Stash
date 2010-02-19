namespace Stash.Specifications.for_configuration.given_graph_context
{
    using System;
    using Configuration;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_told_to_index : with_dummy_graph_context
    {
        [Test]
        public void it_should_complain_if_the_indexer_is_null()
        {
            Index<DummyPersistentObject, object> expected = null;

            typeof(ArgumentNullException)
                .ShouldBeThrownBy(() => Sut.IndexWith(expected));
        }

        [Test]
        public void it_should_register_the_indexer()
        {
            var expected = new DummerIndex();
            Sut.IndexWith(expected);
            Sut.RegisteredGraph.RegisteredIndexers.ShouldContain(
                indexer => ((RegisteredIndexer<DummyPersistentObject, object>)indexer).Index == expected);
        }
    }
}