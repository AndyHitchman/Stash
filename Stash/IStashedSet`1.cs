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
    using System.Collections.Generic;
    using Queries;

    public interface IStashedSet<TGraph> : IEnumerable<TGraph> where TGraph : class
    {
        /// <summary>
        /// Destroy the persistence of a graph.
        /// </summary>
        /// <param name="item"></param>
        void Destroy(TGraph item);

        /// <summary>
        /// Endure a graph by making it persistent.
        /// </summary>
        /// <param name="item"></param>
        void Endure(TGraph item);
        
        /// <summary>
        /// Restrict the stashed set to entities matching the provided constraint.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        IStashedSet<TGraph> Matching(Func<MakeConstraint, IQuery> constraint);

        /// <summary>
        /// Use the stashed set read-only, where no changes to entities are tracked or persisted.
        /// </summary>
        /// <returns></returns>
        IStashedSet<TGraph> Untracked();
    }
}