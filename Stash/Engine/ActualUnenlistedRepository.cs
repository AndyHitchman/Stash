namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using PersistenceEvents;
    using Selectors;

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

        public IEnumerable<Projection<TKey, TProjection>> Fetch<TKey, TProjection>(InternalSession session, From<TKey, TProjection> @from)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TProjection> Fetch<TProjection>(InternalSession session, params From<TProjection>[] @from)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Projection<TKey, TProjection>> Fetch<TKey, TProjection>(From<TKey, TProjection> from)
        {
            return Fetch(getSession(), from);
        }

        public IEnumerable<TProjection> Fetch<TProjection>(params From<TProjection>[] from)
        {
            return Fetch(getSession(), from);
        }

        public Tracker GetTrackerFor<TGraph>(InternalSession session, TGraph graph)
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

        private static InternalSession getSession()
        {
            return Stash.SessionFactory.GetSession().Internalize();
        }
    }
}