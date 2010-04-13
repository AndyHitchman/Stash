namespace Stash.Engine.Serializers.Binary
{
    using System.Collections.Generic;
    using System.Runtime.Serialization.Formatters.Binary;

    public class BinarySerializer<TGraph> : BinarySerializer, ISerializer<TGraph>
    {
        public BinarySerializer() : this(new BinaryFormatter()) {}

        public BinarySerializer(BinaryFormatter binaryFormatter) :base(binaryFormatter)
        {}

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