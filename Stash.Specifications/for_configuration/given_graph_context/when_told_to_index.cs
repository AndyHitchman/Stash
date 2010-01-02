namespace Stash.Specifications.for_configuration.given_graph_context
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_index : with_dummy_graph_context
    {
        [Test]
        public void it_should_complain_if_the_indexer_is_null()
        {
            Indexer<DummyPersistentObject> expected = null;

            typeof(ArgumentNullException)
                .ShouldBeThrownBy(() => Sut.IndexWith(expected));
        }

        [Test]
        public void it_should_register_the_indexer()
        {
            var expected = new DummerIndexer();
            Sut.IndexWith(expected);
            Sut.RegisteredGraph.RegisteredIndexers.ShouldContain(indexer => indexer.Indexer == expected);
        }
    }
}