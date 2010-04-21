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
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Engine;
    using Engine.Serializers;

    /// <summary>
    /// A configured object graph.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class RegisteredGraph<TGraph> : RegisteredGraph, IRegisteredGraph<TGraph>
    {
        private readonly IRegistry registry;
        private IEnumerable<IRegisteredIndexer> registeredIndexersAffectingGraph;

        public RegisteredGraph(IRegistry registry) : base(typeof(TGraph))
        {
            this.registry = registry;
        }

        public override IRegistry Registry
        {
            get { return registry; }
        }

        public override IEnumerable<IRegisteredIndexer> IndexersOnGraph
        {
            get
            {
                return registeredIndexersAffectingGraph ??
                       (registeredIndexersAffectingGraph =
                        Registry
                            .RegisteredIndexers
                            .Where(
                                indexer =>
                                indexer.IndexType
                                    .GetInterfaces()
                                    .Where(iface => typeof(IIndex).IsAssignableFrom(iface))
                                    .Where(iface => iface.IsGenericType)
                                    .Where(iface => iface.GetGenericTypeDefinition().Equals(typeof(IIndexByGraph<>)))
                                    .Any(iface => iface.GetGenericArguments()[0].IsAssignableFrom(typeof(TGraph)))
                            ));
            }
        }

        public ISerializer<TGraph> TransformSerializer { get; set; }

        public override object Deserialize(IEnumerable<byte> serializedGraph, ISerializationSession session)
        {
            return TransformSerializer.Deserialize(serializedGraph, session);
        }

        public override void EngageBackingStore(IBackingStore backingStore) {}

        public IRegisteredIndexer GetRegisteredIndexerFor(IIndex index)
        {
            return IndexersOnGraph.Where(_ => _.IndexType == index.GetType()).First();
        }

        public override IEnumerable<byte> Serialize(object graph, ISerializationSession session)
        {
            return TransformSerializer.Serialize((TGraph)graph, session);
        }
    }
}