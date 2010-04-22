namespace Stash.Engine.Partitioning
{
    using Queries;

    public interface IPartitioningQueryFactory : IQueryFactory
    {
        void AddPartition(IPartition partition, IQueryFactory partitionQueryFactory);
    }
}