namespace Stash.Engine.Serializers
{
    using System.IO;

    public interface Serializer
    {
        Stream Serialize(object graph);
        object Deserialize(Stream serializedGraph);
        TGraph Deserialize<TGraph>(Stream serializedGraph);
    }
}