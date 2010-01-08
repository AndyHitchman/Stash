namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;
    using Engine;

    public class DummyBackingStore : BackingStore
    {
        public void CloseDatabase()
        {
            throw new NotImplementedException();
        }

        public void DeleteGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool EnsureIndex<TGraph, TKey>(Index<TGraph, TKey> index)
        {
            throw new NotImplementedException();
        }

        public bool EnsureMap<TGraph,TKey,TValue>(Map<TGraph,TKey,TValue> map)
        {
            throw new NotImplementedException();
        }

        public bool EnsureReduction<TKey, TValue>(Reduction<TKey, TValue> reduction)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersistentGraph> GetExternallyModifiedGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersistentGraph> GetGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public void InsertGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        public void OpenDatabase()
        {
            throw new NotImplementedException();
        }

        public void UpdateGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }
    }
}