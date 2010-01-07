namespace Stash.Engine
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides access to and management of persistent aggregrate object graphs and derived projections.
    /// </summary>
    public interface UnenlistedRepository : Repository
    {
        /// <summary>
        /// Enumerate all persisted <typeparam name="TGraph"/>
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        IEnumerable<TGraph> All<TGraph>(Session session);

        /// <summary>
        /// Enumerate indexes from the provided <paramref name="indexer"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="indexer"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Session session, Indexer<TGraph, TKey> indexer);

        /// <summary>
        /// Enumerate joined indexes from the provided <paramref name="joinIndexers"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="joinIndexers"></param>
        /// <returns></returns>
        IEnumerable<TGraph> Index<TGraph>(Session session, params Indexer<TGraph>[] joinIndexers);

        /// <summary>
        /// Enumerate mapped projections from the provided <paramref name="mapper"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Session session, Mapper<TGraph> mapper);

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        void Persist<TGraph>(Session session, TGraph graph);

        /// <summary>
        /// Produce the result for the given <paramref name="reducer"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="reducer"></param>
        /// <returns></returns>
        TValue Reduce<TKey, TValue>(Session session, TKey key, Reducer reducer);
    }
}