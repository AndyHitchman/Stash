namespace Stash.Specifications.for_in_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Engine;
    using given_berkeley_backing_store;
    using In.BDB.BerkeleyQueries;
    using Support;

    public class when_not_equal_to : with_int_indexer
    {
        private TrackedGraph equaltrackedGraph;
        private ITrackedGraph notEqualtrackedGraph;
        private TrackedGraph anotherNotEqualtrackedGraph;
        private NotEqualToQuery<int> query;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            equaltrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            notEqualtrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 99)},
                registeredGraph
                );

            anotherNotEqualtrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 101)},
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(equaltrackedGraph);
                        _.InsertGraph(notEqualtrackedGraph);
                        _.InsertGraph(anotherNotEqualtrackedGraph);
                    });

            query = new NotEqualToQuery<int>(registeredIndexer, 100);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(registeredGraph, query));
        }

        [Then]
        public void it_should_get_the_graphs_not_matching_index_key()
        {
            actual.ShouldHaveCount(2);
        }

        [Then]
        public void it_should_get_the_correct_graphs()
        {
            actual.Any(_ => _.InternalId == notEqualtrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == anotherNotEqualtrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}