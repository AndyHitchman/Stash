#region License
// Copyright 2009 Andrew Hitchman
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

    public class StashedSet<TGraph> : IEnumerable<TGraph> where TGraph : class
    {
        private readonly IBackingStore backingStore;
        private readonly IEnumerable<IQuery> queryChain;
        private readonly IQueryFactory queryFactory;
        private readonly IRegistry registry;
        private readonly IInternalSession session;

        public StashedSet(ISession session)
            : this(
                session.Internalize(),
                Kernel.Registry,
                Kernel.Registry.BackingStore,
                Kernel.Registry.BackingStore.QueryFactory,
                typeof(TGraph).Equals(typeof(object))
                    ? Enumerable.Empty<IQuery>()
                    : new[] {Index<StashTypeHierarchy>.EqualTo(StashTypeHierarchy.GetConcreteTypeValue(typeof(TGraph)))}) {}

        public StashedSet(IInternalSession session, IRegistry registry, IBackingStore backingStore, IQueryFactory queryFactory)
            : this(
                session,
                registry,
                backingStore,
                queryFactory,
                Enumerable.Empty<IQuery>()) {}

        public StashedSet(IInternalSession session, IRegistry registry, IBackingStore backingStore, IQueryFactory queryFactory, IEnumerable<IQuery> queryChain)
        {
            this.registry = registry;
            this.backingStore = backingStore;
            this.session = session;
            this.queryFactory = queryFactory;
            this.queryChain = queryChain;
        }

        public IEnumerator<TGraph> GetEnumerator()
        {
            if(!queryChain.Any())
                throw new InvalidOperationException("No queries in query chain");

            return
                backingStore
                    .Get(queryFactory.IntersectionOf(queryChain))
                    .Select(storedGraph => session.Track<TGraph>(storedGraph, registry.GetRegistrationFor(storedGraph.GraphType)))
                    .Select(track => track.Graph)
                    .GetEnumerator();
        }

        public StashedSet<TGraph> Where(IQuery query)
        {
            return new StashedSet<TGraph>(session, registry, backingStore, queryFactory, queryChain.Concat(new[] {query}));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}