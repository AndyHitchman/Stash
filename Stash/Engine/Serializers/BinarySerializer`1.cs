namespace Stash.Engine.Serializers
{
    using System.Collections.Generic;
    using System.Runtime.Serialization.Formatters.Binary;

    public class BinarySerializer<TGraph> : BinarySerializer, ISerializer<TGraph>
    {
        public BinarySerializer() : this(new BinaryFormatter()) {}

        public BinarySerializer(BinaryFormatter binaryFormatter) :base(binaryFormatter)
        {}

        public TGraph Deserialize(IEnumerable<byte> bytes, IInternalSession trackedSession)
        {
            return (TGraph)base.Deserialize(bytes);
        }

        public IEnumerable<byte> Serialize(TGraph graph, IInternalSession trackedSession)
        {
            return base.Serialize(graph);
        }
    }
}