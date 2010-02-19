namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;
    using Engine;

    public class DummyBackingStore : IBackingStore
    {
        public void InsertGraph(ITrackedGraph trackedGraph)
        {
            throw new NotImplementedException();
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            throw new NotImplementedException();
        }

        public void DeleteGraph(Guid internalId)
        {
            throw new NotImplementedException();
        }

        public IStoredGraph Get(Guid internalId)
        {
            throw new NotImplementedException();
        }
    }
}