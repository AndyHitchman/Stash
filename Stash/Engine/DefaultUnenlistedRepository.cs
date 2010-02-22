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

namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Selectors;

    public class DefaultUnenlistedRepository : UnenlistedRepository
    {
        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        public void Delete<TGraph>(TGraph graph) where TGraph : class
        {
            Delete(getSession(), graph);
        }

        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        public void Delete<TGraph>(InternalSession session, TGraph graph) where TGraph : class
        {
            ensureGraphTypeIsRegistered<TGraph>(session);

            session.PersistenceEventFactory.MakeDestroy(Guid.Empty, graph, session).EnrollInSession();
        }

        //        public IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(From<TFromThis, TKey, TProjection> from)
        //            where TFromThis : From<TFromThis, TKey, TProjection>
        //        {
        //            return Fetch(getSession(), from);
        //        }
        //
        //        public IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(
        //            InternalSession session, From<TFromThis, TKey, TProjection> @from)
        //            where TFromThis : From<TFromThis, TKey, TProjection>
        //        {
        //            var fetched = new[] { new Projection<TKey, TProjection>(default(TKey), default(TProjection)) };
        //            foreach(var projection in fetched)
        //            {
        //                session.PersistenceEventFactory.MakeTrack(Guid.Empty, projection.Value, new MemoryStream(), session).EnrollInSession();
        //            }
        //            return fetched;
        //        }

        public IEnumerable<TProjection> Fetch<TFromThis, TProjection>(params From<TFromThis, TProjection>[] from)
            where TFromThis : From<TFromThis, TProjection>
        {
            return Fetch(getSession(), from);
        }

        public IEnumerable<TProjection> Fetch<TFromThis, TProjection>(
            InternalSession session, params From<TFromThis, TProjection>[] @from)
            where TFromThis : From<TFromThis, TProjection>
        {
            var fetched = new[] {default(TProjection)};
            foreach(var projection in fetched)
            {
                session.PersistenceEventFactory.MakeTrack(Guid.Empty, projection, new MemoryStream(), session).EnrollInSession();
            }
            return fetched;
        }

        public Tracker GetTrackerFor<TGraph>(InternalSession session, TGraph graph) where TGraph : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        public void Persist<TGraph>(TGraph graph) where TGraph : class
        {
            Persist(getSession(), graph);
        }

        public void Persist<TGraph>(InternalSession session, TGraph graph) where TGraph : class
        {
            ensureGraphTypeIsRegistered<TGraph>(session);

            session.PersistenceEventFactory.MakeEndure(graph, session).EnrollInSession();
        }

        public void ReconnectTracker(InternalSession session, Tracker tracker)
        {
            throw new NotImplementedException();
        }

        private static void ensureGraphTypeIsRegistered<TGraph>(InternalSession session)
        {
            if(!session.Registry.IsManagingGraphTypeOrAncestor(typeof(TGraph)))
                throw new ArgumentOutOfRangeException("graph", "The graph type is not being managed by Stash");
        }

        private static InternalSession getSession()
        {
            return Stash.SessionFactory.GetSession().Internalize();
        }
    }
}