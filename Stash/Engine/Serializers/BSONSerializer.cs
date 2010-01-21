namespace Stash.Engine.Serializers
{
    using System;
    using System.IO;

    public class BSONSerializer : Serializer
    {
        public Stream Serialize(object graph)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(Stream serializedGraph)
        {
            throw new NotImplementedException();
        }

        public TGraph Deserialize<TGraph>(Stream serializedGraph)
        {
            throw new NotImplementedException();
        }
    }
}