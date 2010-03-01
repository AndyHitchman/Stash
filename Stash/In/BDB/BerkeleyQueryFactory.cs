namespace Stash.In.BDB
{
    using System;
    using System.Collections.Generic;
    using BerkeleyQueries;
    using Configuration;
    using Queries;

    public class BerkeleyQueryFactory : IQueryFactory
    {
        public IAllOfQuery<TKey> AllOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IAnyOfQuery<TKey> AnyOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IBetweenQuery<TKey> Between<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IEqualToQuery<TKey> EqualTo<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new EqualToQuery<TKey>(indexer, key);
        }

        public IGreaterThanQuery<TKey> GreaterThan<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IGreaterThanEqualQuery<TKey> GreaterThanEqual<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IInsideQuery<TKey> Inside<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public ILessThanQuery<TKey> LessThan<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public ILessThanEqualQuery<TKey> LessThanEqual<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public INotAnyOfQuery<TKey> NotAnyOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public INotAnyOfQuery<TKey> NotAllOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IIntersectOperator And(IQuery lhs, IQuery rhs)
        {
            throw new NotImplementedException();
        }

        public IIntersectOperator Or(IQuery lhs, IQuery rhs)
        {
            throw new NotImplementedException();
        }

        public INotEqualToQuery<TKey> NotEqualTo<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IOutsideQuery<TKey> Outside<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }
    }
}