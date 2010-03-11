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
    /// Provides access to and management of persistent aggregrate object graphs and derived projections.
    /// </summary>
    public interface IUnenlistedRepository : IRepository
    {
        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        void Delete<TGraph>(IInternalSession session, TGraph graph) where TGraph : class;

        //        IEnumerable<TProjection> Fetch<TFromThis, TProjection>(InternalSession session, params IFrom<TFromThis, TProjection>[] @from)
        //            where TFromThis : IFrom<TFromThis, TProjection>;

        /// <summary>
        /// Get the <see cref="Tracker"/> for a persisted aggregate object graph.
        /// </summary>
        /// <remarks>
        /// The tracker managed the provided aggregate and allows the aggregrate to be reconnected to a subsequent session.
        /// </remarks>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        Tracker GetTrackerFor<TGraph>(IInternalSession session, TGraph graph) where TGraph : class;

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="session"></param>
        /// <param name="graph"></param>
        void Persist<TGraph>(IInternalSession session, TGraph graph) where TGraph : class;

        /// <summary>
        /// Reconnect a <see cref="Tracker"/> to this session.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="tracker"></param>
        void ReconnectTracker(IInternalSession session, Tracker tracker);

        //        IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(InternalSession session, From<TFromThis, TKey, TProjection> @from) where TFromThis : From<TFromThis, TKey, TProjection>;
    }
}