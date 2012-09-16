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

namespace Stash.Engine.PersistenceEvents
{
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Configuration;

    public class Endure : IEndure
    {
        private readonly IRegisteredGraph registeredGraph;
        private readonly IStoredGraph storedGraph;

        public Endure(object graph, IRegisteredGraph registeredGraph)
        {
            UntypedGraph = graph;
            this.registeredGraph = registeredGraph;
            storedGraph = registeredGraph.CreateStoredGraph();
        }

        public object UntypedGraph { get; private set; }

        public InternalId InternalId
        {
            get { return storedGraph.InternalId; }
        }

        protected virtual IEnumerable<IProjectedIndex> CalculateIndexes()
        {
            return
                registeredGraph.IndexersOnGraph
                    .Select(registeredIndexer => registeredIndexer.GetUntypedProjections(UntypedGraph))
                    .SelectMany(indices => indices);
        }

        public void Complete(IStorageWork work, ISerializationSession session)
        {
            storedGraph.SerialisedGraph = registeredGraph.Serialize(UntypedGraph, session);

            var trackedGraph = new TrackedGraph(
                storedGraph, 
                CalculateIndexes(), 
                registeredGraph);

            work.InsertGraph(trackedGraph);
        }
    }
}