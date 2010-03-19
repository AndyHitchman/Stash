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
    using System.Collections.Generic;
    using BackingStore;

    public interface Map<TGraph, TKey, TValue> : Map<TGraph>
    {
        /// <summary>
        /// The map function that projects the persisted <typeparam name="TGraph">object graph</typeparam> to 
        /// a <typeparam name="TKey">key</typeparam> and <typeparam name="TValue">value</typeparam>.
        /// </summary>
        IEnumerable<IProjectedIndex<TKey>> F(TGraph graph);
    }
}