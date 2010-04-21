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

namespace Stash.Engine.Serializers
{
    using System;
    using System.Collections.Generic;

    public class AdhocSerializer<TGraph> : ISerializer<TGraph>
    {
        private readonly Func<IEnumerable<byte>, TGraph> deserializer;
        private readonly Func<TGraph, IEnumerable<byte>> serializer;

        public AdhocSerializer(Func<TGraph, IEnumerable<byte>> serializer, Func<IEnumerable<byte>, TGraph> deserializer)
        {
            this.serializer = serializer;
            this.deserializer = deserializer;
        }

        public TGraph Deserialize(IEnumerable<byte> bytes, ISerializationSession session)
        {
            return deserializer(bytes);
        }

        public IEnumerable<byte> Serialize(TGraph graph, ISerializationSession session)
        {
            return serializer(graph);
        }
    }
}