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

namespace Stash.Serializers.Binary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Engine;
    using Stash.Configuration;

    /// <summary>
    /// The AggregateBinarySerializer performs standard binary formatting except
    /// where the object to serialise is an instance of a registered graph (which is
    /// intended to represent an aggregate root), in which case a reference by internal id
    /// is serialised. 
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class AggregateBinarySerializer<TGraph> : ISerializer<TGraph>
    {
        private readonly ISurrogateSelector surrogateSelector;
        private readonly IRegisteredGraph<TGraph> registeredGraph;

        public AggregateBinarySerializer(IRegisteredGraph<TGraph> registeredGraph, ISurrogateSelector surrogateSelector)
            : this(registeredGraph)
        {
            this.surrogateSelector = surrogateSelector;
        }

        public AggregateBinarySerializer(IRegisteredGraph<TGraph> registeredGraph)
        {
            surrogateSelector = new SurrogateSelector();
            this.registeredGraph = registeredGraph;
        }

        public TGraph Deserialize(Stream bytes, ISerializationSession session)
        {
            var binarySerializer = getBinarySerializerWithSurrogateSelector(session);
            return (TGraph)binarySerializer.Deserialize(bytes);
        }

        public Stream Serialize(TGraph graph, ISerializationSession session)
        {
            var binarySerializer = getBinarySerializerWithSurrogateSelector(session);
            return binarySerializer.Serialize(graph);
        }

        public string SerializedContentType
        {
            get { return "application/octet-stream"; }
        }

        /// <summary>
        /// Need to use the same context when creating the surrogates as when serialising.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private BinarySerializer getBinarySerializerWithSurrogateSelector(ISerializationSession session)
        {
            var selector = new AggregateSurrogateSelector(registeredGraph);
            selector.ChainSelector(surrogateSelector);
            var streamingContext = new StreamingContext(StreamingContextStates.All, session);
            return new BinarySerializer(new BinaryFormatter(selector, streamingContext));
        }
    }
}