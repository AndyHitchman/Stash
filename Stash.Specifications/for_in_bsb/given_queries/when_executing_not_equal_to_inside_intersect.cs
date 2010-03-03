namespace Stash.Specifications.for_in_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Engine;
    using given_berkeley_backing_store;
    using In.BDB;
    using In.BDB.BerkeleyQueries;
    using Support;

    public class when_executing_not_equal_to_inside_intersect : with_int_indexer
    {
        private IBerkeleyQuery query;
        private IEnumerable<Guid> actual;
        private Guid[] joinConstraint;
        private TrackedGraph equaltrackedGraph;
        private TrackedGraph notEqualtrackedGraph;
        private TrackedGraph anotherNotEqualtrackedGraph;
        private TrackedGraph anotherEqualtrackedGraph;

        protected override void Given()
        {
            equaltrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 100) },
                registeredGraph
                );

            anotherEqualtrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 100) },
                registeredGraph
                );
            
            notEqualtrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 99) },
                registeredGraph
                );

            anotherNotEqualtrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] { new ProjectedIndex<int>(registeredIndexer, 101) },
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                {
                    _.InsertGraph(equaltrackedGraph);
                    _.InsertGraph(anotherEqualtrackedGraph);
                    _.InsertGraph(notEqualtrackedGraph);
                    _.InsertGraph(anotherNotEqualtrackedGraph);
                });

            query = new NotEqualToQuery<int>(registeredIndexer, 100);

            joinConstraint = new[]
                {
                    equaltrackedGraph.InternalId, notEqualtrackedGraph.InternalId, 
                };
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
        public void it_should_find_one()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _ == notEqualtrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}