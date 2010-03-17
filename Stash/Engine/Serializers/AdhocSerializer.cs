namespace Stash.Engine.Serializers
{
    using System;
    using System.Collections.Generic;
    using Configuration;

    public class AdhocSerializer<TGraph> : ISerializer<TGraph>
    {
        private readonly Func<TGraph, RegisteredGraph<TGraph>, IEnumerable<byte>> serializer;
        private readonly Func<IEnumerable<byte>, RegisteredGraph<TGraph>, TGraph> deserializer;

        public AdhocSerializer(Func<TGraph,RegisteredGraph<TGraph>,IEnumerable<byte>> serializer, Func<IEnumerable<byte>,RegisteredGraph<TGraph>,TGraph> deserializer)
        {
            this.serializer = serializer;
            this.deserializer = deserializer;
        }

        public TGraph Deserialize(IEnumerable<byte> bytes, RegisteredGraph<TGraph> registeredGraph)
        {
            return deserializer(bytes, registeredGraph);
        }

        public IEnumerable<byte> Serialize(TGraph graph, RegisteredGraph<TGraph> registeredGraph)
        {
            return serializer(graph, registeredGraph);
        }
    }
}