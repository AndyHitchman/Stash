#region License
// Copyright 2009, 2010 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

namespace Stash
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Engine;
    using Queries;

    public class StashedSet<TGraph> : IStashedSet<TGraph> where TGraph : class
    {
        private readonly IBackingStore backingStore;
        private readonly IEnumerable<StashedSet<TGraph>> queryChain;
        private readonly IQueryFactory queryFactory;
        private readonly IRegistry registry;
        private readonly IInternalSession session;
        private bool untracked;
        private IQuery match;

        public StashedSet(IInternalSession session, IRegistry registry, IBackingStore backingStore, IQueryFactory queryFactory)
            : this(
                session,
                registry,
                backingStore,
                queryFactory,
                Enumerable.Empty<StashedSet<TGraph>>()) {}

        public StashedSet(IInternalSession session, IRegistry registry, IBackingStore backingStore, IQueryFactory queryFactory, IEnumerable<StashedSet<TGraph>> queryChain)
        {
            this.registry = registry;
            this.backingStore = backingStore;
            this.session = session;
            this.queryFactory = queryFactory;
            this.queryChain = queryChain;
        }

        /// <summary>
        /// Destroy the persistence 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void Destroy(TGraph item)
        {
            session.Destroy(item, registry.GetRegistrationFor(item.GetType()));
        }

        /// <summary>
        /// Endure a graph. Make it persistent in the backing store.
        /// </summary>
        /// <param name="item"></param>
        public void Endure(TGraph item)
        {
            session.Endure(item, registry.GetRegistrationFor(item.GetType()));
        }

        public IEnumerator<TGraph> GetEnumerator()
        {
            return 
                GetMatchingStoredGraphs()
                    .Select(
                        storedGraph => 
                            session.Load(
                                storedGraph, 
                                registry.GetRegistrationFor(storedGraph.GraphType), 
                                new SerializationSession(() => session.EnrolledPersistenceEvents, session, untracked), untracked))
                    .Select(track => (TGraph)track.UntypedGraph)
                    .GetEnumerator();
        }

        public IEnumerable<IStoredGraph> GetMatchingStoredGraphs()
        {
            if(!queryChain.Any())
                throw new InvalidOperationException("No queries in query chain");

            return
                backingStore
                    .Get(queryFactory.IntersectionOf(queryChain.Select(_ => _.match)));
        }

        public IStashedSet<TGraph> Matching(Func<MakeConstraint, IQuery> constraint)
        {
            match = constraint(new MakeConstraint(registry, queryFactory));
            return new StashedSet<TGraph>(
                session,
                registry,
                backingStore,
                queryFactory,
                queryChain.Concat(new[] {this}));
        }

        public IStashedSet<TGraph> Untracked()
        {
            untracked = true;
            return this;
        }

        public bool IsUntracked()
        {
            return queryChain.Any(_ => _.untracked);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IStashedSet<TGraph> RestrictToTypeHierarchy()
        {
            return Matching(_ => _.Where<StashTypeHierarchy>().EqualTo(StashTypeHierarchy.GetConcreteTypeValue(typeof(TGraph))));
        }
    }
}