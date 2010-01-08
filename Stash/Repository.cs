namespace Stash
{
    using System.Collections.Generic;

    public interface Repository
    {
        /// <summary>
        /// Enumerate all persisted <typeparam name="TGraph"/>
        /// </summary>
        /// <returns></returns>
        IEnumerable<TGraph> All<TGraph>();

        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        void Delete<TGraph>(TGraph graph);

        /// <summary>
        /// Enumerate indexes from the provided <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Index<TGraph, TKey> index);

        /// <summary>
        /// Enumerate joined indexes from the provided <paramref name="joinIndices"/>.
        /// </summary>
        /// <param name="joinIndices"></param>
        /// <returns></returns>
        IEnumerable<TGraph> Index<TGraph>(params Index<TGraph>[] joinIndices);

        /// <summary>
        /// Enumerate mapped projections from the provided <paramref name="map"/>.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Map<TGraph,TKey,TValue> map);

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        void Persist<TGraph>(TGraph graph);

        /// <summary>
        /// Produce the result for the given <paramref name="reduction"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reduction"></param>
        /// <returns></returns>
        TValue Reduce<TKey, TValue>(TKey key, Reduction<TKey, TValue> reduction);
    }
}