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

namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using Configuration;

    public class StoredGraph : IStoredGraph
    {
        public StoredGraph(Guid internalId, IEnumerable<byte> graph, IRegisteredGraph registeredGraph)
        {
            InternalId = internalId;
            SerialisedGraph = graph;
            RegisteredGraph = registeredGraph;
        }

        public Guid InternalId { get; private set; }
        public IEnumerable<byte> SerialisedGraph { get; private set; }
        public IRegisteredGraph RegisteredGraph { get; set; }

        public Type GraphType
        {
            get { return RegisteredGraph.GraphType; }
        }

        public IEnumerable<string> TypeHierarchy
        {
            get { return RegisteredGraph.TypeHierarchy; }
        }

        public IEnumerable<IRegisteredIndexer> Indexes
        {
            get { return RegisteredGraph.Indexes; }
        }
    }
}