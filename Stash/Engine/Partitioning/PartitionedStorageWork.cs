namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using Configuration;
    using Queries;

    public class PartitionedStorageWork : IStorageWork
    {
        private readonly IPartition partition;
        private readonly IStorageWork underlyingStorageWork;

        public PartitionedStorageWork(IPartition partition, IStorageWork underlyingStorageWork)
        {
            this.partition = partition;
            this.underlyingStorageWork = underlyingStorageWork;
        }

        public int Count(IQuery query)
        {
            return underlyingStorageWork.Count(query);
        }

        public void DeleteGraph(Guid internalId, IRegisteredGraph registeredGraph)
        {
            if (!partition.IsResponsibleForGraph(internalId))
                return;

            underlyingStorageWork.DeleteGraph(internalId, registeredGraph);
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            return underlyingStorageWork.Get(query);
        }

        public IStoredGraph Get(Guid internalId)
        {
            if (!partition.IsResponsibleForGraph(internalId))
                return null;

            try
            {
                return underlyingStorageWork.Get(internalId);
            }
            catch(GraphForKeyNotFoundException)
            {
                //This is normal for partitioned stores.
            }

            return null;
        }

        public void InsertGraph(ITrackedGraph trackedGraph)
        {
            if (!partition.IsResponsibleForGraph(trackedGraph.InternalId))
                return;

            underlyingStorageWork.InsertGraph(trackedGraph);
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            if (!partition.IsResponsibleForGraph(trackedGraph.InternalId))
                return;

            underlyingStorageWork.UpdateGraph(trackedGraph);
        }
    }
}