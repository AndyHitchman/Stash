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
    using System;
    using Engine;

    /// <summary>
    /// The context for configuring a <see cref="Map{TGraph}"/>
    /// </summary>
    /// <typeparam name="TBackingStore"></typeparam>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MapContext<TBackingStore, TGraph, TKey, TValue>
        where TBackingStore : IBackingStore
        where TGraph : class
    {
        public MapContext(RegisteredMapper<TGraph, TKey, TValue> registeredMapper)
        {
            RegisteredMapper = registeredMapper;
        }

        /// <summary>
        /// The configured Map.
        /// </summary>
        public virtual RegisteredMapper<TGraph, TKey, TValue> RegisteredMapper { get; private set; }

        /// <summary>
        /// Instruct the configuration not to persist the map.
        /// The map is transient, and only required as an intermediate step prior for consumption by a <see cref="Reduction"/>.
        /// </summary>
        public virtual void DoNotPersist()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reduce the map with the given <paramref name="reduction"/>
        /// </summary>
        /// <param name="reduction"></param>
        //        public virtual void ReduceWith(Reduction<TKey, TValue> reduction)
        //        {
        //            if(reduction == null) throw new ArgumentNullException("reduction");
        //            RegisteredMapper.RegisteredReducers.Add(new RegisteredReducer<TKey, TValue>(reduction));
        //        }
    }
}