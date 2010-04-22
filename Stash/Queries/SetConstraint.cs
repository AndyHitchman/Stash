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

        /// <summary>
        /// Match graphs that yielded all of the given indexes in the <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        public IAllOfQuery<TKey> AllOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.AllOf(registry.GetIndexerFor<TIndex>(), set);
        }

        /// <summary>
        /// Match graphs that yielded any of the given indexes in the <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        public IAnyOfQuery<TKey> AnyOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.AnyOf(registry.GetIndexerFor<TIndex>(), set);
        }

        /// <summary>
        /// Match graphs that yielded an index inclusively between the <paramref name="lowerKey"/> and <paramref name="upperKey"/> key constraints.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="lowerKey"></param>
        /// <param name="upperKey"></param>
        /// <returns></returns>
        public IBetweenQuery<TKey> Between<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.Between(registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        /// <summary>
        /// Match graphs that yielded an index equal to the <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEqualToQuery<TKey> EqualTo<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.EqualTo(registry.GetIndexerFor<TIndex>(), key);
        }

        /// <summary>
        /// Match graphs that yielded an index greather than the <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IGreaterThanQuery<TKey> GreaterThan<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.GreaterThan(registry.GetIndexerFor<TIndex>(), key);
        }

        /// <summary>
        /// Match graphs that yielded an index greater than or equal to the <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IGreaterThanEqualQuery<TKey> GreaterThanEqual<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.GreaterThanEqual(registry.GetIndexerFor<TIndex>(), key);
        }

        /// <summary>
        /// Match graphs that yielded an index inside the <paramref name="lowerKey"/> and <paramref name="upperKey"/> key constraints.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="lowerKey"></param>
        /// <param name="upperKey"></param>
        /// <returns></returns>
        public IInsideQuery<TKey> Inside<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.Inside(registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        /// <summary>
        /// Match graphs that yielded a key into the index.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public IIsIndexedQuery IsIndexed<TKey>()
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.IsIndexed(registry.GetIndexerFor<TIndex>());
        }

        /// <summary>
        /// Match graphs that yielded an index less than the <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public ILessThanQuery<TKey> LessThan<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.LessThan(registry.GetIndexerFor<TIndex>(), key);
        }

        /// <summary>
        /// Match graphs that yielded an index less than or equal to the <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public ILessThanEqualQuery<TKey> LessThanEqual<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.LessThanEqual(registry.GetIndexerFor<TIndex>(), key);
        }

        /// <summary>
        /// Match graphs that did not yield all of the key <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        public INotAllOfQuery<TKey> NotAllOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.NotAllOf(registry.GetIndexerFor<TIndex>(), set);
        }

        /// <summary>
        /// Match graphs that did not yield any of the key <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        public INotAnyOfQuery<TKey> NotAnyOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.NotAnyOf(registry.GetIndexerFor<TIndex>(), set);
        }

        /// <summary>
        /// Match graphs that did not yield an index equal to the <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public INotEqualToQuery<TKey> NotEqualTo<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.NotEqualTo(registry.GetIndexerFor<TIndex>(), key);
        }

        /// <summary>
        /// Match graphs that yielded an index outside the <paramref name="lowerKey"/> and <paramref name="upperKey"/> key constraints.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="lowerKey"></param>
        /// <param name="upperKey"></param>
        /// <returns></returns>
        public IOutsideQuery<TKey> Outside<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return queryFactory.Outside(registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        /// <summary>
        /// Match graphs that yielded an index string that starts with the <paramref name="key"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IStartsWithQuery StartsWith(string key)
        {
            return queryFactory.StartsWith(registry.GetIndexerFor<TIndex>(), key);
        }

        /// <summary>
        /// Match graphs that did not yield a match for the <paramref name="queryToComplement"/>.
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <param name="queryToComplement"></param>
        /// <returns></returns>
        public TQuery Not<TQuery>(Func<SetConstraint<TIndex>,IComplementaryQuery<TQuery>> queryToComplement) where TQuery : IQuery
        {
            return queryToComplement(this).GetComplementaryQuery();
        }
    }
}