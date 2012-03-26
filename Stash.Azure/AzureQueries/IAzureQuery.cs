namespace Stash.Azure.AzureQueries
{
    using System.Collections.Generic;
    using Engine;
    using Queries;

    public interface IAzureQuery : IQuery
    {
        //QueryCostScale QueryCostScale { get; }
        double EstimatedQueryCost();
        IEnumerable<InternalId> Execute();
        IEnumerable<InternalId> ExecuteInsideIntersect(IEnumerable<InternalId> joinConstraint);
    }
}