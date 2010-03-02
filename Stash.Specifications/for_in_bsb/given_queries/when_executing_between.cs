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

    public class when_executing_between : with_int_indexer
    {
        private TrackedGraph insideTrackedGraph;
        private TrackedGraph lowerTrackedGraph;
        private TrackedGraph upperTrackedGraph;
        private TrackedGraph lessThanTrackedGraph;
        private TrackedGraph greaterThanTrackedGraph;
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            insideTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 101)},
                registeredGraph
                );

            lowerTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            upperTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 102)},
                registeredGraph
                );

            lessThanTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 99)},
                registeredGraph
                );

            greaterThanTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 103)},
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(insideTrackedGraph);
                        _.InsertGraph(lowerTrackedGraph);
                        _.InsertGraph(upperTrackedGraph);
                        _.InsertGraph(lessThanTrackedGraph);
                        _.InsertGraph(greaterThanTrackedGraph);
                    });

            query = new BetweenQuery<int>(registeredIndexer, 100, 102);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(registeredGraph, query));
        }

        [Then]
        public void it_should_find_three()
        {
            actual.ShouldHaveCount(3);
        }

        [Then]
        public void it_should_get_the_correct_graphs()
        {
            actual.Any(_ => _.InternalId == lowerTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == insideTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == upperTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}