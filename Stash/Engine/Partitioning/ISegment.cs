namespace Stash.Engine.Partitioning
{
    using System;

    /// <summary>
    /// Graphs are mapped to segments and then segments to partitions. This allows a segment
    /// to be remapped to a different partition. Typically, create between 100 and 1000 segments
    /// per partition.
    /// </summary>
    public interface ISegment
    {
        IPartition MappedPartition { get; }
        int SegmentNumber { get; }
        bool IsResponsibleForGraph(Guid internalId);
    }
}