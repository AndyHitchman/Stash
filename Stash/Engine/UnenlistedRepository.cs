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
        IEnumerable<TGraph> All<TGraph>(InternalSession session);

        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        void Delete<TGraph>(InternalSession session, TGraph graph);

        /// <summary>
        /// Get the <see cref="Tracker"/> for a persisted aggregate object graph.
        /// </summary>
        /// <remarks>
        /// The tracker managed the provided aggregate and allows the aggregrate to be reconnected to a subsequent session.
        /// </remarks>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        Tracker GetTrackerFor<TGraph>(InternalSession session, TGraph graph);

        /// <summary>
        /// Enumerate indexes from the provided <paramref name="indexer"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="indexer"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(InternalSession session, Indexer<TGraph, TKey> indexer);

        /// <summary>
        /// Enumerate joined indexes from the provided <paramref name="joinIndexers"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="joinIndexers"></param>
        /// <returns></returns>
        IEnumerable<TGraph> Index<TGraph>(InternalSession session, params Indexer<TGraph>[] joinIndexers);

        /// <summary>
        /// Enumerate mapped projections from the provided <paramref name="mapper"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(InternalSession session, Mapper<TGraph> mapper);

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="internalSession"></param>
        /// <param name="graph"></param>
        void Persist<TGraph>(InternalSession internalSession, TGraph graph);

        /// <summary>
        /// Reconnect a <see cref="Tracker"/> to this session.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="tracker"></param>
        void ReconnectTracker(InternalSession session, Tracker tracker);

        /// <summary>
        /// Produce the result for the given <paramref name="reducer"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="reducer"></param>
        /// <returns></returns>
        TValue Reduce<TKey, TValue>(InternalSession session, TKey key, Reducer reducer);
    }
}