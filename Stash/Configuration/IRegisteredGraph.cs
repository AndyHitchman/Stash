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
    using System.Collections.Generic;
    using Engine;

    public interface IRegisteredGraph
    {
        /// <summary>
        /// The <see cref="Type"/> of the root of the object graph.
        /// </summary>
        Type GraphType { get; }

        /// <summary>
        /// The type hierarchy of the <see cref="GraphType"/>, including itself but excluding <see cref="object"/>.
        /// </summary>
        IEnumerable<string> TypeHierarchy { get; }

        /// <summary>
        /// All registered indexes for the graph.
        /// </summary>
        IEnumerable<IRegisteredIndexer> IndexersOnGraph { get; }

        IRegistry Registry { get; }

        object Deserialize(IEnumerable<byte> serializedGraph, IInternalSession session);
        IEnumerable<byte> Serialize(object graph, IInternalSession session);
    }
}