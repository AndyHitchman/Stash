namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Queries;

    public class PartitioningQueryFactory : IQueryFactory
    {
        private readonly Dictionary<IPartition, IQueryFactory> partitionedQueryFactories;

        public PartitioningQueryFactory(Dictionary<IPartition, IQueryFactory> partitionedQueryFactories)
        {
            this.partitionedQueryFactories = partitionedQueryFactories;
        }

        public IAllOfQuery<TKey> AllOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.AllOf(indexer, set));
        }

        public IAnyOfQuery<TKey> AnyOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.AnyOf(indexer, set));
        }

        public IBetweenQuery<TKey> Between<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.Between(indexer, lowerKey, upperKey));
        }

        public IEqualToQuery<TKey> EqualTo<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.EqualTo(indexer, key));
        }

        public IGreaterThanQuery<TKey> GreaterThan<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.GreaterThan(indexer, key));
        }

        public IGreaterThanEqualQuery<TKey> GreaterThanEqual<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.GreaterThanEqual(indexer, key));
        }

        public IInsideQuery<TKey> Inside<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.Inside(indexer, lowerKey, upperKey));
        }

        public IIntersectOperator IntersectionOf(IEnumerable<IQuery> queries)
        {
            return new PartitionedOperator(partitionedQueryFactories, qf => qf.IntersectionOf(queries));
        }

        public IIsIndexedQuery IsIndexed(IRegisteredIndexer indexer)
        {
            return new PartitionedQuery(partitionedQueryFactories, qf => qf.IsIndexed(indexer));
        }

        public ILessThanQuery<TKey> LessThan<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.LessThan(indexer, key));
        }

        public ILessThanEqualQuery<TKey> LessThanEqual<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.LessThanEqual(indexer, key));
        }

        public INotAllOfQuery<TKey> NotAllOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.NotAllOf(indexer, set));
        }

        public INotAnyOfQuery<TKey> NotAnyOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.NotAnyOf(indexer, set));
        }

        public INotEqualToQuery<TKey> NotEqualTo<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.NotEqualTo(indexer, key));
        }

        public IOutsideQuery<TKey> Outside<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new PartitionedQuery<TKey>(partitionedQueryFactories, qf => qf.Outside(indexer, lowerKey, upperKey));
        }

        public IStartsWithQuery StartsWith(IRegisteredIndexer indexer, string key)
        {
            return new PartitionedQuery(partitionedQueryFactories, qf => qf.StartsWith(indexer, key));
        }

        public IUnionOperator UnionOf(IEnumerable<IQuery> queries)
        {
            return new PartitionedOperator(partitionedQueryFactories, qf => qf.UnionOf(queries));
        }
    }
}