namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using PersistenceEvents;
    using Selectors;

    public class DefaultUnenlistedRepository : UnenlistedRepository
    {
        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        public void Delete<TGraph>(TGraph graph) where TGraph : class
        {
            Delete(getSession(), graph);
        }

        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        public void Delete<TGraph>(InternalSession session, TGraph graph) where TGraph : class
        {
            ensureGraphTypeIsRegistered<TGraph>(session);

            new Destroy<TGraph>(graph, session).EnrollInSession();
        }

        private void ensureGraphTypeIsRegistered<TGraph>(InternalSession session)
        {
            if (!session.Registry.IsManagingGraphTypeOrAncestor(typeof(TGraph)))
                throw new ArgumentOutOfRangeException("graph", "The graph type is not being managed by Stash");
        }

        public IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(From<TFromThis, TKey, TProjection> from)
            where TFromThis : From<TFromThis, TKey, TProjection>
        {
            return Fetch(getSession(), from);
        }

        public IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(
            InternalSession session, From<TFromThis, TKey, TProjection> @from)
            where TFromThis : From<TFromThis, TKey, TProjection>
        {
            var fetched = new[] { new Projection<TKey, TProjection>(default(TKey), default(TProjection)) };
            foreach(var projection in fetched)
            {
                new Track<TProjection>(projection.Value, session).EnrollInSession();
            }
            return fetched;
        }

        public IEnumerable<TProjection> Fetch<TFromThis, TProjection>(params From<TFromThis, TProjection>[] from)
            where TFromThis : From<TFromThis, TProjection>
        {
            return Fetch(getSession(), from);
        }

        public IEnumerable<TProjection> Fetch<TFromThis, TProjection>(
            InternalSession session, params From<TFromThis, TProjection>[] @from)
            where TFromThis : From<TFromThis, TProjection>
        {
            var fetched = new[] { default(TProjection) };
            foreach (var projection in fetched)
            {
                new Track<TProjection>(projection, session).EnrollInSession();
            }
            return fetched;
        }

        public Tracker GetTrackerFor<TGraph>(InternalSession session, TGraph graph) where TGraph : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        public void Persist<TGraph>(TGraph graph) where TGraph : class
        {
            Persist(getSession(), graph);
        }

        public void Persist<TGraph>(InternalSession session, TGraph graph) where TGraph : class
        {
            ensureGraphTypeIsRegistered<TGraph>(session);

            new Endure<TGraph>(graph, session).EnrollInSession();
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