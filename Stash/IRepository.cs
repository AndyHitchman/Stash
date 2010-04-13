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
    public interface IRepository
    {
        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        void Delete<TGraph>(TGraph graph) where TGraph : class;

        /// <summary>
        /// Fetch from the projection.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <typeparam name="TFromThis"></typeparam>
        /// <param name="from"></param>
        /// <returns></returns>
        //        IEnumerable<Projection<TKey, TProjection>> Fetch<TFromThis, TKey, TProjection>(From<TFromThis, TKey, TProjection> @from) where TFromThis : From<TFromThis, TKey, TProjection>;
        /// <summary>
        /// Fetch from the set of projections.
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <typeparam name="TFromThis"></typeparam>
        /// <param name="from"></param>
        /// <returns></returns>
        //        IEnumerable<TProjection> Fetch<TFromThis, TProjection>(params IFrom<TFromThis, TProjection>[] @from) where TFromThis : IFrom<TFromThis, TProjection>;
        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        void Persist<TGraph>(TGraph graph) where TGraph : class;
    }
}