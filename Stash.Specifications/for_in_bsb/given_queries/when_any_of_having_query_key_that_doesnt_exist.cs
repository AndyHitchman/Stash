namespace Stash.Specifications.for_in_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Engine;
    using given_berkeley_backing_store;
    using In.BDB.BerkeleyQueries;
    using Queries;
    using Support;

    public class when_any_of_having_query_key_that_doesnt_exist : with_int_indexer
    {
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;
        private TrackedGraph firstMatchingTrackedGraph;
        private TrackedGraph secondMatchingTrackedGraph;
        private TrackedGraph firstNonMatchingTrackedGraph;
        private TrackedGraph secondtNonMatchingTrackedGraph;

        protected override void Given()
        {
            firstMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 101) },
                registeredGraph
                );

            secondMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 99) },
                registeredGraph
                );

            firstNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 100) },
                registeredGraph
                );

            secondtNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 103) },
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                {
                    _.InsertGraph(firstMatchingTrackedGraph);
                    _.InsertGraph(secondMatchingTrackedGraph);
                    _.InsertGraph(firstNonMatchingTrackedGraph);
                    _.InsertGraph(secondtNonMatchingTrackedGraph);
                });

            query = new AnyOfQuery<int>(registeredIndexer, new[] {101, 98, 99});
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(registeredGraph, query));
        }

        [Then]
        public void it_should_find_two()
        {
            actual.ShouldHaveCount(2);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _.InternalId == firstMatchingTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == secondMatchingTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}