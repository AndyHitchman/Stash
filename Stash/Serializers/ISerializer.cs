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

namespace Stash.Serializers
{
    using System.Collections.Generic;
    using System.IO;
    using Engine;

    /// <summary>
    /// An interface that provides serialization and deserialization functions for a graph
    /// Implement to provide customised behaviour or alternative serialization strategies.
    /// </summary>
    public interface ISerializer<TGraph>
    {
        TGraph Deserialize(Stream serial, ISerializationSession session);
        Stream Serialize(TGraph graph, ISerializationSession session);

        string SerializedContentType { get; }
    }
}