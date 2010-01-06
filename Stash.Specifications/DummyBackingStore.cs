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

        public bool EnsureIndexer<TGraph, TKey>(Indexer<TGraph, TKey> indexer)
        {
            throw new NotImplementedException();
        }

        public bool EnsureMapper<TGraph>(Mapper<TGraph> mapper)
        {
            throw new NotImplementedException();
        }

        public bool EnsureReducer(Reducer reducer)
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