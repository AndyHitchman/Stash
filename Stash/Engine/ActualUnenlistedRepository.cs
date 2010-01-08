namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using PersistenceEvents;

    public class ActualUnenlistedRepository : UnenlistedRepository
    {
        /// <summary>
        /// Enumerate all persisted <typeparam name="TGraph"/> with an ad-hoc session.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TGraph> All<TGraph>()
        {
            return All<TGraph>(getSession());
        }

        /// <summary>
        /// Enumerate all persisted <typeparam name="TGraph"/>
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public IEnumerable<TGraph> All<TGraph>(InternalSession session)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        public void Delete<TGraph>(TGraph graph)
        {
            Delete(getSession(), graph);
        }

        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        public void Delete<TGraph>(InternalSession session, TGraph graph)
        {
            session.Enroll(new Destroy<TGraph>(graph));
        }

        public Tracker GetTrackerFor<TGraph>(InternalSession session, TGraph graph)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enumerate indexes from the provided <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Index<TGraph, TKey> index)
        {
            return Index(getSession(), index);
        }

        /// <summary>
        /// Enumerate joined indexes from the provided <paramref name="joinIndices"/>.
        /// </summary>
        /// <param name="joinIndices"></param>
        /// <returns></returns>
        public IEnumerable<TGraph> Index<TGraph>(params Index<TGraph>[] joinIndices)
        {
            return Index(getSession(), joinIndices);
        }

        public IEnumerable<Projection<TKey, TGraph>> Fetch<TGraph, TKey>(InternalSession session, Index<TGraph, TKey> index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TGraph> Fetch<TGraph>(InternalSession session, params Index<TGraph>[] joinIndices)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(InternalSession session, Index<TGraph, TKey> index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TGraph> Index<TGraph>(InternalSession session, params Index<TGraph>[] joinIndices)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enumerate mapped projections from the provided <paramref name="map"/>.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Map<TGraph,TKey,TValue> map)
        {
            return Map<TGraph, TKey, TValue>(getSession(), map);
        }

        public IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(InternalSession session, Map<TGraph,TKey,TValue> map)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        public void Persist<TGraph>(TGraph graph)
        {
            Persist(getSession(), graph);
        }

        public void Persist<TGraph>(InternalSession internalSession, TGraph graph)
        {
            internalSession.Enroll(new Endure<TGraph>(graph));
        }

        public void ReconnectTracker(InternalSession session, Tracker tracker)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Produce the result for the given <paramref name="reduction"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reduction"></param>
        /// <returns></returns>
        public TValue Reduce<TKey, TValue>(TKey key, Reduction<TKey, TValue> reduction)
        {
            return Reduce<TKey, TValue>(getSession(), key, reduction);
        }

        public TValue Reduce<TKey, TValue>(InternalSession session, TKey key, Reduction<TKey, TValue> reduction)
        {
            throw new NotImplementedException();
        }

        private static InternalSession getSession()
        {
            return Stash.SessionFactory.GetSession().Internalize();
        }
    }
}