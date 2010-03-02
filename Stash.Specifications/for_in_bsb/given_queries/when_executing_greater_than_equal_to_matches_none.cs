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

    public class when_executing_greater_than_equal_to_matches_none : with_int_indexer
    {
        private ITrackedGraph lessThanTrackedGraph;
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            lessThanTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 99)},
                registeredGraph
                );

            Subject.InTransactionDo(
                _ => _.InsertGraph(lessThanTrackedGraph));

            query = new GreaterThanEqualToQuery<int>(registeredIndexer, 100);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(registeredGraph, query));
        }

        [Then]
        public void it_should_find_none()
        {
            actual.ShouldHaveCount(0);
        }
    }
}