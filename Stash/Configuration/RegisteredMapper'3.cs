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

namespace Stash.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Engine;

    /// <summary>
    /// A configured Map.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class RegisteredMapper<TGraph, TKey, TValue> : RegisteredMapper<TGraph> where TGraph : class
    {
        public RegisteredMapper(Map<TGraph, TKey, TValue> map)
        {
            Map = map;
            //            RegisteredReducers = new List<RegisteredReducer<TKey, TValue>>();
        }

        /// <summary>
        /// The Map.
        /// </summary>
        public Map<TGraph, TKey, TValue> Map { get; private set; }

        /// <summary>
        /// Reducers chained to the <see cref="Map"/>.
        /// </summary>
        //        public IList<RegisteredReducer<TKey, TValue>> RegisteredReducers { get; private set; }
        public override void EngageBackingStore(IBackingStore backingStore)
        {
            //            backingStore.EnsureMap(Map);
        }

        public override IEnumerable<IProjectedIndex> GetKeyFreeProjections(TGraph graph)
        {
            return Map.F(graph).Cast<IProjectedIndex>();
        }
    }
}