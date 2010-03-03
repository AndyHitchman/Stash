namespace Stash.Specifications.for_in_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Engine;
    using given_berkeley_backing_store;
    using In.BDB;
    using In.BDB.BerkeleyQueries;
    using Queries;
    using Support;

    public class when_looking_at_join_optimisation_performance : with_int_indexer
    {
        private TrackedGraph insideTrackedGraph;
        private TrackedGraph lowerTrackedGraph;
        private TrackedGraph upperTrackedGraph;
        private TrackedGraph lessThanTrackedGraph;
        private TrackedGraph greaterThanTrackedGraph;
        private IBerkeleyQuery query;
        private IEnumerable<Guid> actual;

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
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 103)},
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
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 104)},
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

                        //Generate some records
                        for(var i = 0; i < 2000; i++)
                        {
                            _.InsertGraph(
                                new TrackedGraph(
                                    Guid.NewGuid(),
                                    "letspretendthisisserialiseddata".Select(b => (byte)b),
                                    new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 102)},
                                    registeredGraph
                                    ));
                            
                        }
                    });

            query = new BetweenQuery<int>(registeredIndexer, 100, 103);
        }

        protected override void When()
        {
        }

        [Then]
        public void it_run_faster_with_a_small_number_of_pre_joins()
        {
            var smallJoinConstraint = new[]
                {
                    insideTrackedGraph.InternalId, greaterThanTrackedGraph.InternalId
                }
                .Union(Enumerable.Range(1, 50).Select(_ => Guid.NewGuid()))
                .ToList();
            var largeJoinConstraint = new[]
                {
                    insideTrackedGraph.InternalId, greaterThanTrackedGraph.InternalId,
                }
                .Union(Enumerable.Range(1, 1000).Select(_ => Guid.NewGuid()))
                .ToList();

            //Run once and ignore
            actual = Subject.InTransactionDo(
                _ =>
                {
                    var bsw = (BerkeleyStorageWork)_;
                    return query.ExecuteInsideIntersect(bsw.BackingStore.IndexDatabases[registeredIndexer.IndexName], bsw.Transaction, largeJoinConstraint).ToList();
                });

            //Run once and ignore
            actual = Subject.InTransactionDo(
                _ =>
                {
                    var bsw = (BerkeleyStorageWork)_;
                    return query.ExecuteInsideIntersect(bsw.BackingStore.IndexDatabases[registeredIndexer.IndexName], bsw.Transaction, smallJoinConstraint).ToList();
                });

            var nonOptTime = new Stopwatch();
            nonOptTime.Start();
            actual = Subject.InTransactionDo(
                _ =>
                {
                    var bsw = (BerkeleyStorageWork)_;
                    return query.ExecuteInsideIntersect(bsw.BackingStore.IndexDatabases[registeredIndexer.IndexName], bsw.Transaction, largeJoinConstraint).ToList();
                });
            nonOptTime.Stop();

            var optTime = new Stopwatch();
            optTime.Start();
            actual = Subject.InTransactionDo(
                _ =>
                {
                    var bsw = (BerkeleyStorageWork)_;
                    return query.ExecuteInsideIntersect(bsw.BackingStore.IndexDatabases[registeredIndexer.IndexName], bsw.Transaction, smallJoinConstraint).ToList();
                });
            optTime.Stop();

            Console.WriteLine("Non-opt: " + nonOptTime.ElapsedMilliseconds);
            Console.WriteLine("Opt: " + optTime.ElapsedMilliseconds);

            (optTime.ElapsedMilliseconds < nonOptTime.ElapsedMilliseconds).ShouldBeTrue();
        }
    }
}