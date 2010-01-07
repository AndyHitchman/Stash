namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class ActualEnlistedRepository : EnlistedRepository
    {
        private readonly UnenlistedRepository underlyingUnenlistedRepository;

        public ActualEnlistedRepository(Session enlistToSession, UnenlistedRepository unenlistedRepository)
        {
            underlyingUnenlistedRepository = unenlistedRepository;
            EnlistedSession = enlistToSession;
        }

        /// <summary>
        /// The session this repository is enlisted to.
        /// </summary>
        public Session EnlistedSession { get; private set; }

        public IEnumerable<TGraph> All<TGraph>()
        {
            throw new NotImplementedException();
        }

        public Tracker GetTrackerFor<TGraph>(TGraph graph)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Indexer<TGraph, TKey> indexer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TGraph> Index<TGraph>(params Indexer<TGraph>[] joinIndexers)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Mapper<TGraph> mapper)
        {
            throw new NotImplementedException();
        }

        public void Persist<TGraph>(TGraph graph)
        {
            throw new NotImplementedException();
        }

        public void ReconnectTracker(Tracker tracker)
        {
            throw new NotImplementedException();
        }

        public TValue Reduce<TKey, TValue>(TKey key, Reducer reducer)
        {
            throw new NotImplementedException();
        }
    }
}