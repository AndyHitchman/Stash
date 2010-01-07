namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ActualUnenlistedRepository : UnenlistedRepository
    {
        private Session getSession()
        {
            return Stash.SessionFactory.GetSession();
        }

        /// <summary>
        /// Enumerate all persisted <typeparam name="TGraph"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TGraph> All<TGraph>()
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enumerate joined indexes from the provided <paramref name="joinIndexers"/>.
        /// </summary>
        /// <param name="joinIndexers"></param>
        /// <returns></returns>
        public IEnumerable<TGraph> Index<TGraph>(params Indexer<TGraph>[] joinIndexers)
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        public void Persist<TGraph>(TGraph graph)
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
            throw new NotImplementedException();
        }

        public IEnumerable<TGraph> All<TGraph>(Session session)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Projection<TKey, TGraph>> Index<TGraph, TKey>(Session session, Indexer<TGraph, TKey> indexer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TGraph> Index<TGraph>(Session session, params Indexer<TGraph>[] joinIndexers)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Projection<TKey, TValue>> Map<TGraph, TKey, TValue>(Session session, Mapper<TGraph> mapper)
        {
            throw new NotImplementedException();
        }

        public void Persist<TGraph>(Session session, TGraph graph)
        {
            throw new NotImplementedException();
        }

        public TValue Reduce<TKey, TValue>(Session session, TKey key, Reducer reducer)
        {
            throw new NotImplementedException();
        }
    }
}