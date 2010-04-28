namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;

    public interface IPartitioningStrategy
    {
        PartitioningBackingStore GetPartitioningBackingStore();
        bool IsResponsibleForGraph(ISegment segment, Guid internalId);
    }

    public class PartitioningStrategy : IPartitioningStrategy
    {
        private readonly IEnumerable<IBackingStore> contributingBackingStores;
        private readonly int numberOfSegments;
        private int numberOfBackingStores;

        /// <summary>
        /// A partitioning strategy with 1000 segments. Once the number of segments has been used for allocating graphs
        /// it MUST NOT be varied.
        /// </summary>
        /// <param name="contributingBackingStores"></param>
        public PartitioningStrategy(IEnumerable<IBackingStore> contributingBackingStores) : this(contributingBackingStores, 1000) {}

        /// <summary>
        /// A partitioning strategy with a defined <paramref name="numberOfSegments"/>. Once the number of segments has been used for allocating graphs
        /// it MUST NOT be varied.
        /// </summary>
        /// <param name="contributingBackingStores"></param>
        /// <param name="numberOfSegments"></param>
        public PartitioningStrategy(IEnumerable<IBackingStore> contributingBackingStores, int numberOfSegments)
        {
            numberOfBackingStores = contributingBackingStores.Count();

            if (numberOfSegments < numberOfBackingStores)
                throw new InvalidOperationException("Number of segments must equal or exceed number of contributing backing stored");

            this.contributingBackingStores = contributingBackingStores;
            this.numberOfSegments = numberOfSegments;
        }

        public PartitioningBackingStore GetPartitioningBackingStore()
        {
            throw new NotImplementedException();
        }

        public bool IsResponsibleForGraph(ISegment segment, Guid internalId)
        {
            return internalId.GetHashCode() % numberOfSegments == segment.SegmentNumber - 1;
        }

//        public IDictionary<IPartition,IBackingStore> AllocateBackingStoresToAPartition()
//        {
//            var segments = 
//                Enumerable
//                    .Range(1, numberOfSegments)
//                    .Select(i => new Segment(this, i));
//
//            var partitionedSegments =
//                segments
//                    .GroupBy(_ => (_.SegmentNumber - 1) / (numberOfSegments / numberOfBackingStores));
//            
//            var partitions =
//                contributingBackingStores.ToDictionary(_ => new Partition())
//        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Segment : ISegment 
    {
        private readonly IPartitioningStrategy partitioningStrategy;

        public Segment(IPartitioningStrategy partitioningStrategy, int segmentNumber)
        {
            this.partitioningStrategy = partitioningStrategy;
            SegmentNumber = segmentNumber;
        }

        public int SegmentNumber { get; private set; }

        public IPartition MappedPartition
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsResponsibleForGraph(Guid internalId)
        {
            return partitioningStrategy.IsResponsibleForGraph(this, internalId);
        }
    }

    public class Partition : IPartition 
    {
        public Partition(IEnumerable<ISegment> segmentsServed)
        {
            SegmentsServed = segmentsServed;
        }

        public IEnumerable<ISegment> SegmentsServed { get; private set; }

        public bool IsResponsibleForGraph(Guid internalId)
        {
            return SegmentsServed.Any(_ => _.IsResponsibleForGraph(internalId));
        }
    }
}