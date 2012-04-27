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

namespace Stash.BackingStore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Configuration;
    using Engine;

    public class TrackedGraph : ITrackedGraph
    {
        public TrackedGraph(
            InternalId internalId,
            Stream graph,
            IEnumerable<IProjectedIndex> projectedIndices,
            IRegisteredGraph registeredGraph)
        {
            InternalId = internalId;
            SerialisedGraph = graph;
            ProjectedIndexes = projectedIndices;
            RegisteredGraph = registeredGraph;
        }

        public IEnumerable<IProjectedIndex> ProjectedIndexes { get; private set; }

        public IEnumerable<string> TypeHierarchy
        {
            get { return RegisteredGraph.TypeHierarchy; }
        }

        public Type GraphType
        {
            get { return RegisteredGraph.GraphType; }
        }

        public IEnumerable<IRegisteredIndexer> Indexes
        {
            get { return RegisteredGraph.IndexersOnGraph; }
        }

        public IRegisteredGraph RegisteredGraph { get; private set; }

        public InternalId InternalId { get; private set; }

        public Stream SerialisedGraph { get; private set; }
    }
}