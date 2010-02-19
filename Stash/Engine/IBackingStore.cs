namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public interface IBackingStore
    {
        void InsertGraph(ITrackedGraph trackedGraph);

        void UpdateGraph(ITrackedGraph trackedGraph);

        void DeleteGraph(Guid internalId);

        IStoredGraph Get(Guid internalId);

    }
}