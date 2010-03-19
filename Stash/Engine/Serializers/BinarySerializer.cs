namespace Stash.Engine.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using Configuration;

    public class BinarySerializer<TGraph> : ISerializer<TGraph>
    {
        private readonly BinaryFormatter formatter;

        public BinarySerializer()
        {
            formatter = new BinaryFormatter();
        }

        public TGraph Deserialize(IEnumerable<byte> bytes, RegisteredGraph<TGraph> registeredGraph)
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

        public IEnumerable<byte> Serialize(TGraph graph, RegisteredGraph<TGraph> registeredGraph)
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