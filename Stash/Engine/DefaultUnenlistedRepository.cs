namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using PersistenceEvents;
    using Selectors;

    public class DefaultUnenlistedRepository : UnenlistedRepository
    {
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

        public IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(InternalSession internalSession, From<TFromThis, TKey, TProjection> @from) where TFromThis : From<TFromThis, TKey, TProjection>
        {
            var fetched = new[] { new Projection<TKey, TProjection>(default(TKey), default(TProjection)) };
            foreach(var projection in fetched)
            {
                internalSession.Enroll(new Track<TProjection>(projection.Value));                
            }
            return fetched;
        }

        public IEnumerable<TProjection> Fetch<TFromThis, TProjection>(InternalSession internalSession, params From<TFromThis, TProjection>[] @from) where TFromThis : From<TFromThis, TProjection>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(From<TFromThis, TKey, TProjection> from) where TFromThis : From<TFromThis, TKey, TProjection>
        {
            return Fetch(getSession(), from);
        }

        public IEnumerable<TProjection> Fetch<TFromThis, TProjection>(params From<TFromThis, TProjection>[] from) where TFromThis : From<TFromThis, TProjection>
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