namespace Stash.Engine.Partitioning
{
    using Queries;

    /// <summary>
    /// A partitioned query excutes completely and consistently within a single partition.
    /// Partitioning is governed by the graph, and all indexes are ported with the graph into
    /// the same partition. All queries can be resolved for a single graph, so by having
    /// the graph and it's indexes locally, we do not require and cross-partition resolution.
    /// In particular this greatly simplified Unions and Intersects.
    /// </summary>
    public interface IPartitionedQuery : IQuery
    {
        IQuery GetQueryForPartition(IPartition partition);
    }
}