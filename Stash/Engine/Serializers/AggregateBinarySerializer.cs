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

namespace Stash.Engine.Serializers
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Configuration;

    /// <summary>
    /// The AggregateBinarySerializer performs standard binary formatting except
    /// where the object to serialise is an instance of a registered graph (which is
    /// intended to represent an aggregate root), in which case a reference by internal id
    /// is serialised. 
    /// </summary>
    /// <remarks>
    /// Default serialisation behaviour is to eagerly load and track the aggregate graph. Lazy loading
    /// can be specified in which case a proxy object inheriting from the real class is instantialed. 
    /// This requires virtual methods if the registered graph is a concrete class.
    /// No specific validation of this pre-condition condition is performed.
    /// </remarks>
    /// <typeparam name="TGraph"></typeparam>
    public class AggregateBinarySerializer<TGraph> : ISerializer<TGraph>
    {
        private readonly IRegisteredGraph<TGraph> registeredGraph;

        public AggregateBinarySerializer(IRegisteredGraph<TGraph> registeredGraph)
        {
            this.registeredGraph = registeredGraph;
        }

        public TGraph Deserialize(IEnumerable<byte> bytes, IInternalSession trackedSession)
        {
            var binarySerializer = getBinarySerializerWithSurrogateSelector(trackedSession);
            return (TGraph)binarySerializer.Deserialize(bytes);
        }

        public IEnumerable<byte> Serialize(TGraph graph, IInternalSession trackedSession)
        {
            var binarySerializer = getBinarySerializerWithSurrogateSelector(trackedSession);
            return binarySerializer.Serialize(graph);
        }

        /// <summary>
        /// Need to use the same context when creating the surrogates as when serialising.
        /// </summary>
        /// <param name="trackedSession"></param>
        /// <returns></returns>
        private BinarySerializer getBinarySerializerWithSurrogateSelector(IInternalSession trackedSession)
        {
            var surrogateSelector = new AggregateSurrogateSelector(registeredGraph);
            var streamingContext = new StreamingContext(StreamingContextStates.All, trackedSession);
            return new BinarySerializer(new BinaryFormatter(surrogateSelector, streamingContext));
        }
    }
}