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
            throw new NotImplementedException();
        }

        INotAnyOfQuery<TKey> IComplementaryQuery<INotAnyOfQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }

        IOutsideQuery<TKey> IComplementaryQuery<IOutsideQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }

        INotEqualToQuery<TKey> IComplementaryQuery<INotEqualToQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }

        ILessThanEqualQuery<TKey> IComplementaryQuery<ILessThanEqualQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }

        ILessThanQuery<TKey> IComplementaryQuery<ILessThanQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }

        IEqualToQuery<TKey> IComplementaryQuery<IEqualToQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }

        IAllOfQuery<TKey> IComplementaryQuery<IAllOfQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }

        IGreaterThanQuery<TKey> IComplementaryQuery<IGreaterThanQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }

        IGreaterThanEqualQuery<TKey> IComplementaryQuery<IGreaterThanEqualQuery<TKey>>.GetComplementaryQuery()
        {
            throw new NotImplementedException();
        }
    }
}