namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public interface BackingStore : IDisposable
    {
        void InsertGraphs(IEnumerable<PersistentGraph> persistentGraphs);
        void UpdateGraphs(IEnumerable<PersistentGraph> persistentGraphs);
        void DeleteGraphs(IEnumerable<Guid> internalIds);
        IEnumerable<PersistentGraph> GetGraphs(IEnumerable<Guid> internalIds);
        IEnumerable<PersistentGraph> GetExternallyModifiedGraphs(IEnumerable<PersistentGraph> persistentGraphs);
    }
}