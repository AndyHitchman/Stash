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

namespace Stash.Engine.Serializers.Binary
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    public class BinarySerializer
    {
        private readonly BinaryFormatter binaryFormatter;

        public BinarySerializer(BinaryFormatter binaryFormatter)
        {
            this.binaryFormatter = binaryFormatter;
        }

        public object Deserialize(IEnumerable<byte> bytes)
        {
            var stream = new MemoryStream(bytes.ToArray());
            try
            {
                return binaryFormatter.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public IEnumerable<byte> Serialize(object graph)
        {
            var stream = new MemoryStream();
            try
            {
                binaryFormatter.Serialize(stream, graph);
                return stream.ToArray();
            }
            finally
            {
                stream.Close();
            }
        }
    }
}