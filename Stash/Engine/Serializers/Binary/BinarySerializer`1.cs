﻿#region License
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

namespace Stash.Engine.Serializers.Binary
{
    using System.Collections.Generic;
    using System.Runtime.Serialization.Formatters.Binary;

    public class BinarySerializer<TGraph> : BinarySerializer, ISerializer<TGraph>
    {
        public BinarySerializer() : this(new BinaryFormatter()) {}

        public BinarySerializer(BinaryFormatter binaryFormatter) : base(binaryFormatter) {}

        public TGraph Deserialize(IEnumerable<byte> bytes, IInternalSession session)
        {
            return (TGraph)base.Deserialize(bytes);
        }

        public IEnumerable<byte> Serialize(TGraph graph, IInternalSession session)
        {
            return base.Serialize(graph);
        }
    }
}