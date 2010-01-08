namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

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

        public IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Indexer<TGraph, TKey> indexer)
        {
            return underlyingUnenlistedRepository.Index(enlistedSession, indexer);
        }

        public IEnumerable<TGraph> Index<TGraph>(params Indexer<TGraph>[] joinIndexers)
        {
            return underlyingUnenlistedRepository.Index(enlistedSession, joinIndexers);
        }

        public IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Mapper<TGraph> mapper)
        {
            return underlyingUnenlistedRepository.Map<TGraph, TKey, TValue>(enlistedSession, mapper);
        }

        public void Persist<TGraph>(TGraph graph)
        {
            underlyingUnenlistedRepository.Persist(enlistedSession, graph);
        }

        public void ReconnectTracker(Tracker tracker)
        {
            underlyingUnenlistedRepository.ReconnectTracker(enlistedSession, tracker);
        }

        public TValue Reduce<TKey, TValue>(TKey key, Reducer reducer)
        {
            return underlyingUnenlistedRepository.Reduce<TKey, TValue>(enlistedSession, key, reducer);
        }
    }
}