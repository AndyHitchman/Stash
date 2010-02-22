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
    using Engine;

    /// <summary>
    /// A configured object graph.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class RegisteredGraph<TGraph> : RegisteredGraph
    {
        public RegisteredGraph() : base(typeof(TGraph))
        {
            RegisteredIndexers = new List<RegisteredIndexer<TGraph>>();
            RegisteredMappers = new List<RegisteredMapper<TGraph>>();
        }

        public virtual IList<RegisteredIndexer<TGraph>> RegisteredIndexers { get; private set; }
        public virtual IList<RegisteredMapper<TGraph>> RegisteredMappers { get; private set; }

        public override void EngageBackingStore(IBackingStore backingStore)
        {
            foreach(var registeredIndexer in RegisteredIndexers)
                registeredIndexer.EngageBackingStore(backingStore);
        }
    }
}