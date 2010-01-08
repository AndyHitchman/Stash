namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public interface BackingStore : IDisposable
    {
        void DeleteGraphs(IEnumerable<Guid> internalIds);

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="index"></param>
        /// <returns>true is the backing store is going to manage the Index, false if the backing store expects management to be by the Stash Engine.</returns>
        bool EnsureIndex<TGraph, TKey>(Index<TGraph, TKey> index);

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map"></param>
        /// <returns>true is the backing store is going to manage the Map, false if the backing store expects management to be by the Stash Engine.</returns>
        bool EnsureMap<TGraph,TKey,TValue>(Map<TGraph,TKey,TValue> map);

        /// <summary>
        /// Ensures that the database is configured to persist the provided <paramref name="reduction"/>.
        /// </summary>
        /// <param name="reduction"></param>
        /// <returns>true is the backing store is going to manage the Reduction, false if the backing store expects management to be by the Stash Engine.</returns>
        bool EnsureReduction(Reduction reduction);

        IEnumerable<PersistentGraph> GetExternallyModifiedGraphs(IEnumerable<PersistentGraph> persistentGraphs);
        IEnumerable<PersistentGraph> GetGraphs(IEnumerable<Guid> internalIds);
        void InsertGraphs(IEnumerable<PersistentGraph> persistentGraphs);
        void UpdateGraphs(IEnumerable<PersistentGraph> persistentGraphs);
    }
}