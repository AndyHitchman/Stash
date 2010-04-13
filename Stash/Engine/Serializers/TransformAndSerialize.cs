﻿namespace Stash.Engine.Serializers
{
    using System;
    using System.Collections.Generic;

    public class TransformAndSerialize<TGraph,TTransform> : ISerializer<TGraph>
    {
        public ITransformer<TGraph, TTransform> Transformer { get; private set; }
        public ISerializer<TTransform> Serializer { get; private set; }

        public TransformAndSerialize(ITransformer<TGraph,TTransform> transformer, ISerializer<TTransform> serializer)
        {
            Transformer = transformer;
            Serializer = serializer;
        }

        public TGraph Deserialize(IEnumerable<byte> bytes, IInternalSession trackedSession)
        {
            return Transformer.TransformUp(Serializer.Deserialize(bytes, trackedSession));
        }

        public IEnumerable<byte> Serialize(TGraph graph, IInternalSession trackedSession)
        {
            return Serializer.Serialize(Transformer.TransformDown(graph), trackedSession);
        }
    }
}