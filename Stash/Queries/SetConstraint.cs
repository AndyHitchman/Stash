#region License
// Copyright 2009, 2010 Andrew Hitchman
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

namespace Stash.Queries
{
    using System;
    using System.Collections.Generic;
    using Configuration;

    public class SetConstraint<TIndex> where TIndex : IIndex
    {
        private readonly IQueryFactory queryFactory;
        private readonly IRegistry registry;

        public SetConstraint(IRegistry registry, IQueryFactory queryFactory)
        {
            this.registry = registry;
            this.queryFactory = queryFactory;
        }

        public IAllOfQuery<TKey> AllOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.AllOf(registry.GetIndexerFor<TIndex>(), set);
        }

        public IAnyOfQuery<TKey> AnyOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.AnyOf(registry.GetIndexerFor<TIndex>(), set);
        }

        public IBetweenQuery<TKey> Between<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.Between(registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        public IEqualToQuery<TKey> EqualTo<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.EqualTo(registry.GetIndexerFor<TIndex>(), key);
        }

        public IGreaterThanQuery<TKey> GreaterThan<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.GreaterThan(registry.GetIndexerFor<TIndex>(), key);
        }

        public IGreaterThanEqualQuery<TKey> GreaterThanEqual<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.GreaterThanEqual(registry.GetIndexerFor<TIndex>(), key);
        }

        public IInsideQuery<TKey> Inside<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.Inside(registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        public IIsIndexedQuery IsIndexed<TKey>()
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.IsIndexed(registry.GetIndexerFor<TIndex>());
        }

        public ILessThanQuery<TKey> LessThan<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.LessThan(registry.GetIndexerFor<TIndex>(), key);
        }

        public ILessThanEqualQuery<TKey> LessThanEqual<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.LessThanEqual(registry.GetIndexerFor<TIndex>(), key);
        }

        public INotAnyOfQuery<TKey> NotAllOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.NotAllOf(registry.GetIndexerFor<TIndex>(), set);
        }

        public INotAnyOfQuery<TKey> NotAnyOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.NotAnyOf(registry.GetIndexerFor<TIndex>(), set);
        }

        public INotEqualToQuery<TKey> NotEqualTo<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.NotEqualTo(registry.GetIndexerFor<TIndex>(), key);
        }

        public IOutsideQuery<TKey> Outside<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.Outside(registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        public IStartsWithQuery StartsWith(string key)
        {
            return queryFactory.StartsWith(registry.GetIndexerFor<TIndex>(), key);
        }
    }
}