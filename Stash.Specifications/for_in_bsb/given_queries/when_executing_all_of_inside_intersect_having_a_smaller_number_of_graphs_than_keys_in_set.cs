namespace Stash.Specifications.for_in_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Engine;
    using given_berkeley_backing_store;
    using In.BDB;
    using In.BDB.BerkeleyQueries;
    using Queries;
    using Support;

    public class when_executing_all_of_inside_intersect_having_a_smaller_number_of_graphs_than_keys_in_set : with_int_indexer
    {
        private IBerkeleyQuery query;
        private IEnumerable<Guid> actual;
        private TrackedGraph firstMatchingTrackedGraph;
        private TrackedGraph secondMatchingTrackedGraph;
        private TrackedGraph matchingButExcludedByIntersectTrackedGraph;
        private TrackedGraph nonMatchingTrackedGraph;
        private Guid[] joinConstraint;

        protected override void Given()
        {
            firstMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 102), new ProjectedIndex<int>(registeredIndexer, 101), new ProjectedIndex<int>(registeredIndexer, 100) },
                registeredGraph
                );

            secondMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 102), new ProjectedIndex<int>(registeredIndexer, 99), new ProjectedIndex<int>(registeredIndexer, 101), new ProjectedIndex<int>(registeredIndexer, 100) },
                registeredGraph
                );

            matchingButExcludedByIntersectTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 100), new ProjectedIndex<int>(registeredIndexer, 101) },
                registeredGraph
                );

            nonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 101) },
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                {
                    _.InsertGraph(firstMatchingTrackedGraph);
                    _.InsertGraph(secondMatchingTrackedGraph);
                    _.InsertGraph(matchingButExcludedByIntersectTrackedGraph);
                    _.InsertGraph(nonMatchingTrackedGraph);
                });

            query = new AllOfQuery<int>(registeredIndexer, new[] { 102, 100, 101 });

            joinConstraint = new[] {firstMatchingTrackedGraph.InternalId, secondMatchingTrackedGraph.InternalId};
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var bsw = (BerkeleyStorageWork)_;
                        return query.ExecuteInsideIntersect(bsw.BackingStore.IndexDatabases[registeredIndexer.IndexName], bsw.Transaction, joinConstraint).ToList();
                    });
        }

        [Then]
        public void it_should_find_two()
        {
            actual.ShouldHaveCount(2);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _ == firstMatchingTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _ == secondMatchingTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}