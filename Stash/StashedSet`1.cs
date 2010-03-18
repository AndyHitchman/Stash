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
    using Configuration;
    using Engine;
    using Queries;
    using System.Linq;

    public class StashedSet<TGraph> : IEnumerable<TGraph>
    {
        private readonly IEnumerable<IQuery> queryChain;

        public StashedSet() : this(Kernel.Registry, Kernel.SessionFactory.GetSession()) {}

        public StashedSet(IRegistry registry, ISession session)
        {
            Registry = registry;
            Session = session;
        }

        public StashedSet(IRegistry registry, ISession session, IEnumerable<IQuery> queryChain)
        {
            this.queryChain = queryChain;
            Registry = registry;
            Session = session;
        }

        public IRegistry Registry { get; private set; }
        public ISession Session { get; private set; }
        private IInternalSession internalSession { get { return Session.Internalize(); }}

        public StashedSet<TGraph> Where(IQuery query)
        {
            return new StashedSet<TGraph>(Registry, Session, queryChain.Concat(new[] {query}));
        }

        public IEnumerator<TGraph> GetEnumerator()
        {
            if(!queryChain.Any())
                throw new InvalidOperationException("No query specified in Where()");

            var registeredGraph = Registry.GetRegistrationFor<TGraph>();
            foreach(var storedGraph in Registry.BackingStore.Get(Index.IntersectionOf(queryChain)))
            {
                var track = internalSession.PersistenceEventFactory.MakeTrack(storedGraph, registeredGraph);
                internalSession.Enroll(track);
                yield return track.Graph;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}