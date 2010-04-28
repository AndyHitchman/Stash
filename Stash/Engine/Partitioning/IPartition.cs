namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using BackingStore;

    public interface IPartition
    {
        IEnumerable<ISegment> SegmentsServed { get; }
        bool IsResponsibleForGraph(Guid internalId);
    }
}