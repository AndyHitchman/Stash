namespace Stash.Engine.Serializers
{
    using System;
    using System.Collections.Generic;
    using Configuration;

    /// <summary>
    /// The AggregateBinarySerializer performs standard binary formatting except
    /// where the object to serialise is an instance of a registered graph (which is
    /// intended to represent an aggregate root), in which case a reference by internal id
    /// is serialised. 
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class AggregateBinarySerializer<TGraph> : ISerializer<TGraph>
    {
        public AggregateBinarySerializer()
        {
            
        }

        public TGraph Deserialize(IEnumerable<byte> bytes, RegisteredGraph<TGraph> registeredGraph)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> Serialize(TGraph graph, RegisteredGraph<TGraph> registeredGraph)
        {
            throw new NotImplementedException();
        }
    }
}