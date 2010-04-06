namespace Stash.Engine.Serializers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The AggregateBinarySerializer performs standard binary formatting except
    /// where the object to serialise is an instance of a registered graph (which is
    /// intended to represent an aggregate root), in which case a reference by internal id
    /// is serialised. 
    /// </summary>
    /// <remarks>
    /// Default serialisation behaviour is to eagerly load and track the aggregate graph. Lazy loading
    /// can be specified in which case a proxy object inheriting from the real class is instantialed. 
    /// This requires virtual methods if the registered graph is a concrete class.
    /// No specific validation of this pre-condition condition is performed.
    /// </remarks>
    /// <typeparam name="TGraph"></typeparam>
    public class AggregateBinarySerializer<TGraph> : ISerializer<TGraph>
    {
        public AggregateBinarySerializer()
        {
            
        }

        public TGraph Deserialize(IEnumerable<byte> bytes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> Serialize(TGraph graph)
        {
            throw new NotImplementedException();
        }
    }
}