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
        /// Enumerate indexes from the provided <paramref name="indexer"/>.
        /// </summary>
        /// <param name="indexer"></param>
        /// <returns></returns>
        public IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Indexer<TGraph, TKey> indexer)
        {
            return Index(getSession(), indexer);
        }

        /// <summary>
        /// Enumerate joined indexes from the provided <paramref name="joinIndexers"/>.
        /// </summary>
        /// <param name="joinIndexers"></param>
        /// <returns></returns>
        public IEnumerable<TGraph> Index<TGraph>(params Indexer<TGraph>[] joinIndexers)
        {
            return Index(getSession(), joinIndexers);
        }

        public IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(InternalSession session, Indexer<TGraph, TKey> indexer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TGraph> Index<TGraph>(InternalSession session, params Indexer<TGraph>[] joinIndexers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enumerate mapped projections from the provided <paramref name="mapper"/>.
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Mapper<TGraph> mapper)
        {
            return Map<TGraph, TKey, TValue>(getSession(), mapper);
        }

        public IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(InternalSession session, Mapper<TGraph> mapper)
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
        /// Produce the result for the given <paramref name="reducer"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reducer"></param>
        /// <returns></returns>
        public TValue Reduce<TKey, TValue>(TKey key, Reducer reducer)
        {
            return Reduce<TKey, TValue>(getSession(), key, reducer);
        }

        public TValue Reduce<TKey, TValue>(InternalSession session, TKey key, Reducer reducer)
        {
            throw new NotImplementedException();
        }

        private static InternalSession getSession()
        {
            return Stash.SessionFactory.GetSession().Internalize();
        }
    }
}