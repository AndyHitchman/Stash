namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using Selectors;

    /// <summary>
    /// The default implementation of <see cref="EnlistedRepository"/> delegates all requests to a
    /// provided <see cref="UnenlistedRepository"/> using the <see cref="EnlistedSession"/>.
    /// </summary>
    public class DefaultEnlistedRepository : EnlistedRepository
    {
        private readonly InternalSession enlistedSession;
        private readonly UnenlistedRepository underlyingUnenlistedRepository;

        public DefaultEnlistedRepository(InternalSession enlistToSession, UnenlistedRepository unenlistedRepository)
        {
            underlyingUnenlistedRepository = unenlistedRepository;
            enlistedSession = enlistToSession;
        }

        /// <summary>
        /// The session this repository is enlisted to.
        /// </summary>
        public Session EnlistedSession
        {
            get { return enlistedSession; }
        }

        public void Delete<TGraph>(TGraph graph) where TGraph : class
        {
            underlyingUnenlistedRepository.Delete(enlistedSession, graph);
        }

        public Tracker GetTrackerFor<TGraph>(TGraph graph) where TGraph : class
        {
            return underlyingUnenlistedRepository.GetTrackerFor(enlistedSession, graph);
        }

        public void Persist<TGraph>(TGraph graph) where TGraph : class
        {
            underlyingUnenlistedRepository.Persist(enlistedSession, graph);
        }

//        public IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(From<TFromThis, TKey, TProjection> from) where TFromThis : From<TFromThis, TKey, TProjection>
//        {
//            return underlyingUnenlistedRepository.Fetch(enlistedSession, from);
//        }

        public IEnumerable<TProjection> Fetch<TFromThis,TProjection>(params From<TFromThis, TProjection>[] from) where TFromThis : From<TFromThis, TProjection>
        {
            return underlyingUnenlistedRepository.Fetch(enlistedSession, from);
        }

        public void ReconnectTracker(Tracker tracker)
        {
            underlyingUnenlistedRepository.ReconnectTracker(enlistedSession, tracker);
        }
    }
}