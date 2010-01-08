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
        /// Enumerate indexes from the provided <paramref name="indexer"/>.
        /// </summary>
        /// <param name="indexer"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Indexer<TGraph, TKey> indexer);

        /// <summary>
        /// Enumerate joined indexes from the provided <paramref name="joinIndexers"/>.
        /// </summary>
        /// <param name="joinIndexers"></param>
        /// <returns></returns>
        IEnumerable<TGraph> Index<TGraph>(params Indexer<TGraph>[] joinIndexers);

        /// <summary>
        /// Enumerate mapped projections from the provided <paramref name="mapper"/>.
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Mapper<TGraph> mapper);

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        void Persist<TGraph>(TGraph graph);

        /// <summary>
        /// Produce the result for the given <paramref name="reducer"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reducer"></param>
        /// <returns></returns>
        TValue Reduce<TKey, TValue>(TKey key, Reducer reducer);
    }
}