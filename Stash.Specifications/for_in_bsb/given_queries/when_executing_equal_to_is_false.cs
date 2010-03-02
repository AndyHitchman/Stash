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

    public class when_executing_equal_to_is_false : with_int_indexer
    {
        private ITrackedGraph trackedGraph;
        private IQuery query;
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

            query = new EqualToQuery<int>(registeredIndexer, 99);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(registeredGraph, query));
        }

        [Then]
        public void it_should_not_find_anything()
        {
            actual.ShouldHaveCount(0);
        }
    }
}