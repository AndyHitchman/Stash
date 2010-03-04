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
    using System.Linq;
    using BackingStore;
    using Engine;
    using Engine.Serializers;

    public class Registry
    {
        public Registry()
        {
            RegisteredGraphs = new Dictionary<Type, RegisteredGraph>();
        }

        public Dictionary<Type, RegisteredGraph> RegisteredGraphs { get; private set; }

        /// <summary>
        /// The aggregate object graphs currently configured.
        /// </summary>
        public virtual IEnumerable<RegisteredGraph> AllRegisteredGraphs
        {
            get { return RegisteredGraphs.Values; }
        }

        public virtual IBackingStore BackingStore { get; private set; }

        public virtual Func<Serializer> Serializer { get; set; }

        /// <summary>
        /// Engage the backing store in managing the stash.
        /// </summary>
        public virtual void EngageBackingStore(IBackingStore backingStore)
        {
            BackingStore = backingStore;
            foreach(var registeredGraph in AllRegisteredGraphs)
            {
                registeredGraph.EngageBackingStore(backingStore);
            }
        }

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <typeparamref name="TGraph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <returns></returns>
        public virtual RegisteredGraph<TGraph> GetRegistrationFor<TGraph>()
        {
            return (RegisteredGraph<TGraph>)GetRegistrationFor(typeof(TGraph));
        }

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <paramref name="graphType"/>.
        /// </summary>
        /// <returns></returns>
        public virtual RegisteredGraph GetRegistrationFor(Type graphType)
        {
            if(graphType == null) throw new ArgumentNullException("graphType");
            if(!RegisteredGraphs.ContainsKey(graphType)) throw new ArgumentOutOfRangeException("graphType");

            return RegisteredGraphs[graphType];
        }

        public virtual bool IsManagingGraphTypeOrAncestor(Type graphType)
        {
            return AllRegisteredGraphs.Any(rg => rg.GraphType.IsAssignableFrom(graphType));
        }

        public virtual RegisteredGraph<TGraph> RegisterGraph<TGraph>() where TGraph : class
        {
            var graph = typeof(TGraph);
            if(RegisteredGraphs.ContainsKey(graph))
                throw new ArgumentException(string.Format("Graph {0} is already registered", graph));

            var registeredGraph = new RegisteredGraph<TGraph>();
            RegisteredGraphs.Add(graph, registeredGraph);
            return registeredGraph;
        }
    }
}