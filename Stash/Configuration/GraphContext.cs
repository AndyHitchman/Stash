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
    using Engine;
    using Engine.Serializers;

    /// <summary>
    /// The context for configuring a persistent object graph.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class GraphContext<TGraph> where TGraph : class
    {
        public GraphContext(RegisteredGraph<TGraph> registeredGraph)
        {
            RegisteredGraph = registeredGraph;
        }

        /// <summary>
        /// The configured object graph.
        /// </summary>
        public virtual RegisteredGraph<TGraph> RegisteredGraph { get; private set; }

        /// <summary>
        /// Tell the engine to use the provided serializaton functions implemented by the <paramref name="serializer"/>.
        /// </summary>
        /// <param name="serializer"></param>
        public virtual void SerializeWith(ISerializer<TGraph> serializer)
        {
            throw new NotImplementedException();
        }
    }
}