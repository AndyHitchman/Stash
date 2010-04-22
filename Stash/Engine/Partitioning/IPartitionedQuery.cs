namespace Stash.Engine.Partitioning
{
    using Queries;

    public interface IPartitionedQuery : IQuery
    {
        IQuery GetQueryForPartition(IPartition partition);
    }
}