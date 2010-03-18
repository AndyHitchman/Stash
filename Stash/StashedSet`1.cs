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
    using BackingStore;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using Queries;
    using System.Linq;

    public class StashedSet<TGraph> : IEnumerable<TGraph> where TGraph : class
    {
        private readonly IRegistry registry;
        private readonly IBackingStore backingStore;
        private readonly IInternalSession session;
        private readonly IQueryFactory queryFactory;
        private readonly IEnumerable<IQuery> queryChain;

        public StashedSet(ISession session) : this(session.Internalize(), Kernel.Registry, Kernel.Registry.BackingStore, Kernel.Registry.BackingStore.QueryFactory) {}

        public StashedSet(IInternalSession session, IRegistry registry, IBackingStore backingStore, IQueryFactory queryFactory)
        {
            this.registry = registry;
            this.backingStore = backingStore;
            this.session = session;
            this.queryFactory = queryFactory;
            queryChain = Enumerable.Empty<IQuery>();
        }

        public StashedSet(IInternalSession session, IRegistry registry, IBackingStore backingStore, IQueryFactory queryFactory, IEnumerable<IQuery> queryChain)
        {
            this.registry = registry;
            this.backingStore = backingStore;
            this.session = session;
            this.queryFactory = queryFactory;
            this.queryChain = queryChain;
        }

        public StashedSet<TGraph> Where(IQuery query)
        {
            return new StashedSet<TGraph>(session, registry, backingStore, queryFactory, queryChain.Concat(new[] {query}));
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}