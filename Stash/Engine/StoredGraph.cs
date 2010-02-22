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

    public class StoredGraph : IStoredGraph
    {
        protected StoredGraph(Guid internalId, IEnumerable<byte> graph, Type concreteType, IEnumerable<Type> superTypes)
        {
            InternalId = internalId;
            SerialisedGraph = graph;
            ConcreteType = concreteType;
            SuperTypes = superTypes;
        }

        public Guid InternalId { get; private set; }
        public IEnumerable<byte> SerialisedGraph { get; private set; }
        public Type ConcreteType { get; private set; }
        public IEnumerable<Type> SuperTypes { get; private set; }
    }
}