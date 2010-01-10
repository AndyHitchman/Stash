namespace Stash.Engine
{
    using System.Collections.Generic;
    using Selectors;

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

        IEnumerable<Projection<TKey, TProjection>> Fetch<TKey, TProjection>(InternalSession session, From<TKey, TProjection> @from);

        IEnumerable<TProjection> Fetch<TProjection>(InternalSession session, params From<TProjection>[] @from);
    }
}