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

namespace Stash.Configuration
{
    using System;
    using BackingStore;

    /// <summary>
    /// The root context for configuring persistence.
    /// </summary>
    /// <typeparam name="TBackingStore"></typeparam>
    public class PersistenceContext<TBackingStore> where TBackingStore : IBackingStore
    {
        public PersistenceContext(IRegistry registry)
        {
            Registry = registry;
        }

        public IRegistry Registry { get; private set; }

        /// <summary>
        /// Index the object graph with the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TGraph"></typeparam>
        public virtual void Index<TGraph, TKey>(IIndex<TGraph, TKey> index) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            if(index == null) throw new ArgumentNullException("index");
            var registeredIndexer = new RegisteredIndexer<TGraph, TKey>(index);
            Registry.RegisterIndexer(registeredIndexer);
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> and provide an action that performs additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="configurePersistentGraph"></param>
        public virtual void Register<TGraph>(Action<GraphContext<TGraph>> configurePersistentGraph) where TGraph : class
        {
            configurePersistentGraph(new GraphContext<TGraph>(Registry.RegisterGraph<TGraph>()));
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> with no additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public virtual void Register<TGraph>() where TGraph : class
        {
            Registry.RegisterGraph<TGraph>();
        }
    }
}