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

namespace Stash.Serializers
{
    using System;
    using System.IO;
    using Engine;

    public class TransformAndSerialize<TGraph, TTransform> : ISerializer<TGraph>
    {
        public TransformAndSerialize(ITransformer<TGraph, TTransform> transformer, ISerializer<TTransform> serializer)
        {
            Transformer = transformer;
            Serializer = serializer;
        }

        public ITransformer<TGraph, TTransform> Transformer { get; private set; }
        public ISerializer<TTransform> Serializer { get; private set; }

        public TGraph Deserialize(Stream serial, ISerializationSession session)
        {
            return Transformer.TransformUp(Serializer.Deserialize(serial, session));
        }

        public Stream Serialize(TGraph graph, ISerializationSession session)
        {
            return Serializer.Serialize(Transformer.TransformDown(graph), session);
        }

        public string SerializedContentType
        {
            get { return Serializer.SerializedContentType; }
        }
    }
}