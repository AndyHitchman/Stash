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

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="indexer"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="indexer"></param>
        /// <returns>true is the backing store is going to manage the indexer, false if the backing store expects management to be by the Stash Engine.</returns>
        bool EnsureIndexer<TGraph,TKey>(Indexer<TGraph,TKey> indexer);

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="mapper"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="mapper"></param>
        /// <returns>true is the backing store is going to manage the mapper, false if the backing store expects management to be by the Stash Engine.</returns>
        bool EnsureMapper<TGraph>(Mapper<TGraph> mapper);

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="reducer"/>.
        /// </summary>
        /// <param name="reducer"></param>
        /// <returns>true is the backing store is going to manage the reducer, false if the backing store expects management to be by the Stash Engine.</returns>
        bool EnsureReducer(Reducer reducer);
    }
}