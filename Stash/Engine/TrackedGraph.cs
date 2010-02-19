using System;
using System.Collections.Generic;

namespace Stash.Engine
{
    public class TrackedGraph : StoredGraph, ITrackedGraph
    {
        public TrackedGraph(
                Guid internalId,
                IEnumerable<byte> graph,
                Type concreteType,
                IEnumerable<Type> superTypes,
                IEnumerable<IProjectedIndex> indexes) 
            : base(internalId, graph, concreteType, superTypes)
        {
            Indexes = indexes;
        }

        public IEnumerable<IProjectedIndex> Indexes { get; private set; }
    }
}