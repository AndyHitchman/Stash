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
    using Configuration;

    public static class Where
    {
        public static IAllOfQuery<TKey> AllOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.AllOf<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), set);
        }

        public static IAnyOfQuery<TKey> AnyOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.AnyOf<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), set);
        }

        public static IBetweenQuery<TKey> Between<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.Between<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), lowerKey, upperKey);
        }

        public static IEqualToQuery<TKey> EqualTo<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.EqualTo<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static IGreaterThanQuery<TKey> GreaterThan<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.GreaterThan<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static IGreaterThanEqualQuery<TKey> GreaterThanEqual<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.GreaterThanEqual<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static IInsideQuery<TKey> Inside<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.Inside<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), lowerKey, upperKey);
        }

        public static ILessThanQuery<TKey> LessThan<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.LessThan<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static ILessThanEqualQuery<TKey> LessThanEqual<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.LessThanEqual<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static INotAnyOfQuery<TKey> NotAnyOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.NotAnyOf<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), set);
        }

        public static INotAnyOfQuery<TKey> NotAllOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.NotAllOf<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), set);
        }

        public static INotEqualToQuery<TKey> NotEqualTo<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.NotEqualTo<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static IOutsideQuery<TKey> Outside<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.BackingStore.QueryFactory.Outside<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), lowerKey, upperKey);
        }

        public static IIntersectOperator And(this IQuery lhs, IQuery rhs)
        {
            return Stash.Registry.BackingStore.QueryFactory.And(lhs, rhs);
        }

        public static IIntersectOperator Or(this IQuery lhs, IQuery rhs)
        {
            return Stash.Registry.BackingStore.QueryFactory.Or(lhs, rhs);
        }

        private static IRegisteredIndexer getRegisteredIndexer<TGraph, TKey>() where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Stash.Registry.GetRegistrationFor<TGraph>().GetRegisteredIndexerFor<IIndex<TGraph, TKey>>();
        }
    }
}