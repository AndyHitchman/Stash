namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Queries;

    public class PartitionedBackingStore : IBackingStore
    {
        private readonly IBackingStore underlyingBackingStore;
        
        public PartitionedBackingStore(IPartition partition, IBackingStore underlyingBackingStore)
        {
            Partition = partition;
            this.underlyingBackingStore = underlyingBackingStore;
        }

        public IPartition Partition { get; private set; }

        public IQueryFactory QueryFactory
        {
            get { return underlyingBackingStore.QueryFactory; }
        }

        public void Close()
        {
            underlyingBackingStore.Close();
        }

        public int Count(IQuery query)
        {
            var count = 0;

            InTransactionDo(
                work =>
                    {
                        count = work.Count(query);
                    });

            return count;
        }

        public void EnsureIndex(IRegisteredIndexer registeredIndexer)
        {
            underlyingBackingStore.EnsureIndex(registeredIndexer);
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            var storedGraphs = Enumerable.Empty<IStoredGraph>();

            InTransactionDo(
                work =>
                {
                    storedGraphs = work.Get(query);
                });

            return storedGraphs;
        }

        public IStoredGraph Get(Guid internalId)
        {
            IStoredGraph storedGraph = null;

            InTransactionDo(
                work =>
                {
                    storedGraph = work.Get(internalId);
                });

            return storedGraph;
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            //Filter all storage work actions through the partitioned storage worker,
            //which is responsible for determining whether its partition should do
            //anything with the action.
            underlyingBackingStore.InTransactionDo(
                work =>
                    {
                        var partitionedWork = new PartitionedStorageWork(work);
                        storageWorkActions(partitionedWork);
                    });
        }
    }
}