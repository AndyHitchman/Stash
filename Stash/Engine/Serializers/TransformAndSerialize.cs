namespace Stash.Engine.Serializers
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

        public TGraph Deserialize(IEnumerable<byte> bytes)
        {
            return Transformer.TransformUp(Serializer.Deserialize(bytes));
        }

        public IEnumerable<byte> Serialize(TGraph graph)
        {
            return Serializer.Serialize(Transformer.TransformDown(graph));
        }
    }
}