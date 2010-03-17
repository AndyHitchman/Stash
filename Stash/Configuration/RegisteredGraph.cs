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
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using BackingStore;
    using Engine;

    /// <summary>
    /// An abstract configured graph.
    /// </summary>
    public abstract class RegisteredGraph : IRegisteredGraph
    {
        private IEnumerable<string> superTypes;

        protected RegisteredGraph(Type aggregateType)
        {
            GraphType = aggregateType;
        }

        /// <summary>
        /// The <see cref="Type"/> of the root of the object graph.
        /// </summary>
        public virtual Type GraphType { get; private set; }

        public IEnumerable<string> TypeHierarchy
        {
            get
            {
                if(superTypes != null) return superTypes;

                superTypes = new StashTypeHierarchy().Yield(GraphType);
                return superTypes;
            }
        }

        public abstract IEnumerable<IRegisteredIndexer> IndexersOnGraph { get; }
        public abstract Registry Registry { get; }

        public abstract void EngageBackingStore(IBackingStore backingStore);
    }
}