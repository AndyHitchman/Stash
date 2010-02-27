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

    public class when_estimating_scan_cost : with_int_indexer
    {
        private TrackedGraph equaltrackedGraph;
        private ITrackedGraph lessThanTrackedGraph;
        private TrackedGraph greaterThanTrackedGraph;
        private GreaterThanQuery<int> query;
        private double actual;

        protected override void Given()
        {
            equaltrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
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
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 101)},
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(equaltrackedGraph);
                        _.InsertGraph(lessThanTrackedGraph);
                        _.InsertGraph(greaterThanTrackedGraph);
                    });

            query = new GreaterThanQuery<int>(registeredIndexer, 100);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ =>
                                                 {
                                                     var berkeleyStorageWork = ((BerkeleyStorageWork)_);
                                                     return query.EstimatedScanCost(
                                                         berkeleyStorageWork.BackingStore.IndexDatabases[registeredIndexer.IndexName], 
                                                         berkeleyStorageWork.Transaction);
                                                 });
            Console.WriteLine(actual);
        }
    }
}