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

namespace Stash.Queries
{
    using System;
    using System.Collections.Generic;

    public static class Index
    {
        public static IAllOfQuery<TKey> AllOf<TKey>(this IIndexByKey<TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.AllOf(Kernel.Registry.GetIndexerFor(index), set);
        }

        /// <summary>
        /// A synonym for <see cref="IntersectionOf"/>.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static IIntersectOperator And(this IQuery lhs, IQuery rhs)
        {
            return IntersectionOf(lhs, rhs);
        }

        public static IAnyOfQuery<TKey> AnyOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.AnyOf(Kernel.Registry.GetIndexerFor(index), set);
        }

        public static IBetweenQuery<TKey> Between<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.Between(Kernel.Registry.GetIndexerFor(index), lowerKey, upperKey);
        }

        public static IEqualToQuery<TKey> EqualTo<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.EqualTo(Kernel.Registry.GetIndexerFor(index), key);
        }

        public static IGreaterThanQuery<TKey> GreaterThan<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.GreaterThan(Kernel.Registry.GetIndexerFor(index), key);
        }

        public static IGreaterThanEqualQuery<TKey> GreaterThanEqual<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.GreaterThanEqual(Kernel.Registry.GetIndexerFor(index), key);
        }

        public static IInsideQuery<TKey> Inside<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.Inside(Kernel.Registry.GetIndexerFor(index), lowerKey, upperKey);
        }

        public static IIntersectOperator IntersectionOf(IEnumerable<IQuery> queries)
        {
            return Kernel.Registry.BackingStore.QueryFactory.IntersectionOf(queries);
        }

        public static IIntersectOperator IntersectionOf(IQuery lhs, IQuery rhs)
        {
            return IntersectionOf(new[] {lhs, rhs});
        }

        public static IIsIndexedQuery IsIndexed<TGraph, TKey>(this IIndex<TGraph, TKey> index)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.IsIndexed(Kernel.Registry.GetIndexerFor(index));
        }

        public static ILessThanQuery<TKey> LessThan<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.LessThan(Kernel.Registry.GetIndexerFor(index), key);
        }

        public static ILessThanEqualQuery<TKey> LessThanEqual<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.LessThanEqual(Kernel.Registry.GetIndexerFor(index), key);
        }

        public static INotAnyOfQuery<TKey> NotAllOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.NotAllOf(Kernel.Registry.GetIndexerFor(index), set);
        }

        public static INotAnyOfQuery<TKey> NotAnyOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.NotAnyOf(Kernel.Registry.GetIndexerFor(index), set);
        }

        public static INotEqualToQuery<TKey> NotEqualTo<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.NotEqualTo(Kernel.Registry.GetIndexerFor(index), key);
        }

        public static IUnionOperator Or(this IQuery lhs, IQuery rhs)
        {
            return UnionOf(lhs, rhs);
        }

        public static IOutsideQuery<TKey> Outside<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.Outside(Kernel.Registry.GetIndexerFor(index), lowerKey, upperKey);
        }

        public static IUnionOperator UnionOf(IQuery lhs, IQuery rhs)
        {
            return Kernel.Registry.BackingStore.QueryFactory.UnionOf(new[] {lhs, rhs});
        }

        public static IUnionOperator UnionOf(IEnumerable<IQuery> queries)
        {
            return Kernel.Registry.BackingStore.QueryFactory.UnionOf(queries);
        }
    }
}