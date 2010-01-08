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

        public IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Index<TGraph, TKey> index)
        {
            return underlyingUnenlistedRepository.Index(enlistedSession, index);
        }

        public IEnumerable<TGraph> Index<TGraph>(params Index<TGraph>[] joinIndices)
        {
            return underlyingUnenlistedRepository.Index(enlistedSession, joinIndices);
        }

        public IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Map<TGraph, TKey, TValue> map)
        {
            return underlyingUnenlistedRepository.Map<TGraph, TKey, TValue>(enlistedSession, map);
        }

        public void Persist<TGraph>(TGraph graph)
        {
            underlyingUnenlistedRepository.Persist(enlistedSession, graph);
        }

        public void ReconnectTracker(Tracker tracker)
        {
            underlyingUnenlistedRepository.ReconnectTracker(enlistedSession, tracker);
        }

        public TValue Reduce<TKey, TValue>(TKey key, Reduction<TKey, TValue> reduction)
        {
            return underlyingUnenlistedRepository.Reduce<TKey, TValue>(enlistedSession, key, reduction);
        }
    }
}