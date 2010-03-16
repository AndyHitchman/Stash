#region License

// Copyright 2009 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.

#endregion

namespace Stash.BackingStore.BDB
{
    using System;
    using System.Collections.Generic;
    using BerkeleyQueries;
    using Configuration;
    using Queries;

    public class BerkeleyQueryFactory : IQueryFactory
    {
        public IAllOfQuery<TKey> AllOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new AllOfQuery<TKey>(indexer, set);
        }

        public IIntersectOperator IntersectionOf(IEnumerable<IQuery> queries)
        {
            return new IntersectOperator(queries);
        }

        public IAnyOfQuery<TKey> AnyOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new AnyOfQuery<TKey>(indexer, set);
        }

        public IBetweenQuery<TKey> Between<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new BetweenQuery<TKey>(indexer, lowerKey, upperKey);
        }

        public IEqualToQuery<TKey> EqualTo<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new EqualToQuery<TKey>(indexer, key);
        }

        public IGreaterThanQuery<TKey> GreaterThan<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new GreaterThanQuery<TKey>(indexer, key);
        }

        public IGreaterThanEqualQuery<TKey> GreaterThanEqual<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new GreaterThanEqualToQuery<TKey>(indexer, key);
        }

        public IInsideQuery<TKey> Inside<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new InsideQuery<TKey>(indexer, lowerKey, upperKey);
        }

        public ILessThanQuery<TKey> LessThan<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new LessThanQuery<TKey>(indexer, key);
        }

        public ILessThanEqualQuery<TKey> LessThanEqual<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new LessThanEqualToQuery<TKey>(indexer, key);
        }

        public INotAnyOfQuery<TKey> NotAllOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new NotAnyOfQuery<TKey>(indexer, set);
        }

        public INotAnyOfQuery<TKey> NotAnyOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new NotAnyOfQuery<TKey>(indexer, set);
        }

        public INotEqualToQuery<TKey> NotEqualTo<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new NotEqualToQuery<TKey>(indexer, key);
        }

        public IUnionOperator UnionOf(IEnumerable<IQuery> queries)
        {
            return new UnionOperator(queries);
        }

        public IOutsideQuery<TKey> Outside<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new OutsideQuery<TKey>(indexer, lowerKey, upperKey);
        }

        public IIsIndexedQuery IsIndexed(IRegisteredIndexer indexer)
        {
            return new IsIndexedQuery(indexer);
        }
    }
}