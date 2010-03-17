namespace Stash.Configuration
{
    using System.Collections.Generic;
    using Engine.Serializers;

    public interface IRegisteredGraph<TGraph> : IRegisteredGraph
    {
        ISerializer<TGraph> Serializer { get; set; }
        IEnumerable<byte> Serialize(TGraph graph);
        TGraph Deserialize(IEnumerable<byte> serializedGraph);
    }
}