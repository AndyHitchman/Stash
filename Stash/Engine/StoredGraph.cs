using System;
using System.Collections.Generic;

namespace Stash.Engine
{
    public class StoredGraph : IStoredGraph
    {
        protected StoredGraph(Guid internalId, IEnumerable<byte> graph, Type concreteType, IEnumerable<Type> superTypes)
        {
            InternalId = internalId;
            SerialisedGraph = graph;
            ConcreteType = concreteType;
            SuperTypes = superTypes;
        }

        public Guid InternalId { get; private set; }
        public IEnumerable<byte> SerialisedGraph { get; private set; }
        public Type ConcreteType { get; private set; }
        public IEnumerable<Type> SuperTypes { get; private set; }
    }
}