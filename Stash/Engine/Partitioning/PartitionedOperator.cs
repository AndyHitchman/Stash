﻿namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using Queries;

    public class PartitionedOperator : PartitionedQuery, IUnionOperator, IIntersectOperator, INotOperator
    {
        public PartitionedOperator(Dictionary<IPartition, IQueryFactory> partitionedQueryFactories, Func<IQueryFactory, IQuery> getPartitionedQuery)
            : base(partitionedQueryFactories, getPartitionedQuery) {}
    }
}