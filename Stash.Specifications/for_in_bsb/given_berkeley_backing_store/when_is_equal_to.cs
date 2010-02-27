namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Engine;
    using In.BDB.BerkeleyQueries;
    using Support;

    public class when_is_equal_to : with_int_indexer
    {
        private ITrackedGraph trackedGraph;
        private BerkeleyEqualToQuery<int> equalToQuery;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            trackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));

            equalToQuery = new BerkeleyEqualToQuery<int>(registeredIndexer, 100);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(registeredGraph, equalToQuery));
        }

        [Then]
        public void it_should_get_the_only_graph_with_the_matching_index_key()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.First().InternalId.ShouldEqual(trackedGraph.InternalId);
        }
     }
}