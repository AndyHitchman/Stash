using System;
using System.Collections.Generic;

namespace Stash.Engine
{
    public interface ITrackedGraph : IStoredGraph
    {
        IEnumerable<IProjectedIndex> Indexes { get; }
    }
}