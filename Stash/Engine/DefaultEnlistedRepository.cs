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
    /// <summary>
    /// The default implementation of <see cref="EnlistedRepository"/> delegates all requests to a
    /// provided <see cref="IUnenlistedRepository"/> using the <see cref="EnlistedSession"/>.
    /// </summary>
    public class DefaultEnlistedRepository : EnlistedRepository
    {
        private readonly IInternalSession enlistedSession;
        private readonly IUnenlistedRepository underlyingUnenlistedRepository;

        public DefaultEnlistedRepository(IInternalSession enlistToSession, IUnenlistedRepository unenlistedRepository)
        {
            underlyingUnenlistedRepository = unenlistedRepository;
            enlistedSession = enlistToSession;
        }

        /// <summary>
        /// The session this repository is enlisted to.
        /// </summary>
        public ISession EnlistedSession
        {
            get { return enlistedSession; }
        }

        public void Delete<TGraph>(TGraph graph) where TGraph : class
        {
            underlyingUnenlistedRepository.Delete(enlistedSession, graph);
        }

        //        public IEnumerable<TProjection> Fetch<TFromThis, TProjection>(params IFrom<TFromThis, TProjection>[] from) where TFromThis : IFrom<TFromThis, TProjection>
        //        {
        //            return underlyingUnenlistedRepository.Fetch(enlistedSession, from);
        //        }

        public Tracker GetTrackerFor<TGraph>(TGraph graph) where TGraph : class
        {
            return underlyingUnenlistedRepository.GetTrackerFor(enlistedSession, graph);
        }

        public void Persist<TGraph>(TGraph graph) where TGraph : class
        {
            underlyingUnenlistedRepository.Persist(enlistedSession, graph);
        }

        //        public IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(From<TFromThis, TKey, TProjection> from) where TFromThis : From<TFromThis, TKey, TProjection>
        //        {
        //            return underlyingUnenlistedRepository.Fetch(enlistedSession, from);
        //        }

        public void ReconnectTracker(Tracker tracker)
        {
            underlyingUnenlistedRepository.ReconnectTracker(enlistedSession, tracker);
        }
    }
}