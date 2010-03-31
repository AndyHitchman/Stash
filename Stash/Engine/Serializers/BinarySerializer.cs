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
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using Configuration;

    public class BinarySerializer<TGraph> : ISerializer<TGraph>
    {
        private readonly BinaryFormatter formatter;

        public BinarySerializer() : this(new BinaryFormatter()) {}

        public BinarySerializer(BinaryFormatter binaryFormatter)
        {
            formatter = binaryFormatter;
        }

        public TGraph Deserialize(IEnumerable<byte> bytes, IRegisteredGraph<TGraph> registeredGraph)
        {
            var stream = new MemoryStream(bytes.ToArray());
            try
            {
                return (TGraph)formatter.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public IEnumerable<byte> Serialize(TGraph graph, IRegisteredGraph<TGraph> registeredGraph)
        {
            var stream = new MemoryStream();
            try
            {
                formatter.Serialize(stream, graph);
                return stream.ToArray();
            }
            finally
            {
                stream.Close();
            }
        }
    }
}