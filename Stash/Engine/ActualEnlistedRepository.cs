namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using Selectors;

    public class ActualEnlistedRepository : EnlistedRepository
    {
        private readonly InternalSession enlistedSession;
        private readonly UnenlistedRepository underlyingUnenlistedRepository;

        public ActualEnlistedRepository(InternalSession enlistToSession, UnenlistedRepository unenlistedRepository)
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

        public IEnumerable<TGraph> All<TGraph>()
        {
            return underlyingUnenlistedRepository.All<TGraph>(enlistedSession);
        }

        public void Delete<TGraph>(TGraph graph)
        {
            underlyingUnenlistedRepository.Delete(enlistedSession, graph);
        }

        public Tracker GetTrackerFor<TGraph>(TGraph graph)
        {
            return underlyingUnenlistedRepository.GetTrackerFor(enlistedSession, graph);
        }

        public void Persist<TGraph>(TGraph graph)
        {
            underlyingUnenlistedRepository.Persist(enlistedSession, graph);
        }

        public IEnumerable<Projection<TKey, TProjection>> Fetch<TKey, TProjection>(From<TKey, TProjection> from)
        {
            return underlyingUnenlistedRepository.Fetch(enlistedSession, from);
        }

        public IEnumerable<TProjection> Fetch<TProjection>(params From<TProjection>[] from)
        {
            return underlyingUnenlistedRepository.Fetch(enlistedSession, from);
        }

        public void ReconnectTracker(Tracker tracker)
        {
            underlyingUnenlistedRepository.ReconnectTracker(enlistedSession, tracker);
        }
    }
}