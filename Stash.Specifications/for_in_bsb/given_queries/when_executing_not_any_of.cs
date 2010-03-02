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

    public class when_executing_not_any_of : with_int_indexer
    {
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;
        private TrackedGraph thirdNonMatchingTrackedGraph;
        private TrackedGraph secondNonMatchingTrackedGraph;
        private TrackedGraph firstNonMatchingTrackedGraph;
        private TrackedGraph matchingTrackedGraph;

        protected override void Given()
        {
            firstNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 100) },
                registeredGraph
                );
            
            secondNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 99), new ProjectedIndex<int>(registeredIndexer, 101), new ProjectedIndex<int>(registeredIndexer, 100) },
                registeredGraph
                );
            
            thirdNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 101), new ProjectedIndex<int>(registeredIndexer, 100) },
                registeredGraph
                );

            matchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 101) },
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                {
                    _.InsertGraph(thirdNonMatchingTrackedGraph);
                    _.InsertGraph(secondNonMatchingTrackedGraph);
                    _.InsertGraph(firstNonMatchingTrackedGraph);
                    _.InsertGraph(matchingTrackedGraph);
                });

            query = new NotAnyOfQuery<int>(registeredIndexer, new[] {99, 100});
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(registeredGraph, query));
        }

        [Then]
        public void it_should_find_one()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _.InternalId == matchingTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}