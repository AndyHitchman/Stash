namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using Configuration;
    using Queries;

    public class PartitionedStorageWork : IStorageWork
    {
        private readonly IStorageWork underlyingStorageWork;

        public PartitionedStorageWork(IStorageWork underlyingStorageWork)
        {
            this.underlyingStorageWork = underlyingStorageWork;
        }

        public int Count(IQuery query)
        {
            return underlyingStorageWork.Count(query);
        }

        public void DeleteGraph(Guid internalId, IRegisteredGraph registeredGraph)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            return underlyingStorageWork.Get(query);
        }

        public IStoredGraph Get(Guid internalId)
        {
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
            throw new NotImplementedException();
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            throw new NotImplementedException();
        }
    }
}