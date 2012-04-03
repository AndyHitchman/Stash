namespace Stash.Azure.AzureQueries
{
    using System.Collections.Generic;
    using Engine;
    using Microsoft.WindowsAzure.StorageClient;
    using Queries;

    public interface IAzureQuery : IQuery
    {
        QueryCostScale QueryCostScale { get; }
        double EstimatedQueryCost(TableServiceContext serviceContext);
        IEnumerable<InternalId> Execute(TableServiceContext serviceContext);
        IEnumerable<InternalId> ExecuteInsideIntersect(TableServiceContext serviceContext, IEnumerable<InternalId> joinConstraint);
    }
}