namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using Queries;

    public class PartitionedQuery<TKey> : PartitionedQuery,
                                          IAllOfQuery<TKey>,
                                          IAnyOfQuery<TKey>,
                                          IBetweenQuery<TKey>,
                                          IEqualToQuery<TKey>,
                                          IGreaterThanQuery<TKey>,
                                          IGreaterThanEqualQuery<TKey>,
                                          IInsideQuery<TKey>,
                                          IOutsideQuery<TKey>,
                                          INotEqualToQuery<TKey>,
                                          INotAnyOfQuery<TKey>,
                                          INotAllOfQuery<TKey>,
                                          ILessThanEqualQuery<TKey>,
                                          ILessThanQuery<TKey> where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        public PartitionedQuery(IDictionary<IPartition, IQueryFactory> partitionedQueryFactories, Func<IQueryFactory, IQuery> getPartitionedQuery)
            : base(partitionedQueryFactories, getPartitionedQuery) {}

        INotAllOfQuery<TKey> IComplementaryQuery<INotAllOfQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<INotAllOfQuery<TKey>>,INotAllOfQuery<TKey>>);
        }

        INotAnyOfQuery<TKey> IComplementaryQuery<INotAnyOfQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<INotAnyOfQuery<TKey>>, INotAnyOfQuery<TKey>>);
        }

        IOutsideQuery<TKey> IComplementaryQuery<IOutsideQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<IOutsideQuery<TKey>>, IOutsideQuery<TKey>>);
        }

        INotEqualToQuery<TKey> IComplementaryQuery<INotEqualToQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<INotEqualToQuery<TKey>>, INotEqualToQuery<TKey>>);
        }

        ILessThanEqualQuery<TKey> IComplementaryQuery<ILessThanEqualQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<ILessThanEqualQuery<TKey>>, ILessThanEqualQuery<TKey>>);
        }

        ILessThanQuery<TKey> IComplementaryQuery<ILessThanQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<ILessThanQuery<TKey>>, ILessThanQuery<TKey>>);
        }

        IEqualToQuery<TKey> IComplementaryQuery<IEqualToQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<IEqualToQuery<TKey>>, IEqualToQuery<TKey>>);
        }

        IAllOfQuery<TKey> IComplementaryQuery<IAllOfQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<IAllOfQuery<TKey>>, IAllOfQuery<TKey>>);
        }

        IGreaterThanQuery<TKey> IComplementaryQuery<IGreaterThanQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<IGreaterThanQuery<TKey>>, IGreaterThanQuery<TKey>>);
        }

        IGreaterThanEqualQuery<TKey> IComplementaryQuery<IGreaterThanEqualQuery<TKey>>.GetComplementaryQuery()
        {
            return new PartitionedQuery<TKey>(PartitionedQueryFactories, complementQuery<IComplementaryQuery<IGreaterThanEqualQuery<TKey>>, IGreaterThanEqualQuery<TKey>>);
        }

        private TQuery complementQuery<TComplement,TQuery>(IQueryFactory qf) where TComplement : IComplementaryQuery<TQuery> where TQuery : IQuery
        {
            return ((TComplement)GetPartitionedQuery(qf)).GetComplementaryQuery();
        }
    }
}