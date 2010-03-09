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
            return Kernel.Registry.BackingStore.Query.AllOf<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), set);
        }

        public static IIntersectOperator And(this IQuery lhs, IQuery rhs)
        {
            return Kernel.Registry.BackingStore.Query.And(lhs, rhs);
        }

        public static IAnyOfQuery<TKey> AnyOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.AnyOf<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), set);
        }

        public static IBetweenQuery<TKey> Between<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.Between<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), lowerKey, upperKey);
        }

        public static IEqualToQuery<TKey> EqualTo<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.EqualTo<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static IGreaterThanQuery<TKey> GreaterThan<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.GreaterThan<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static IGreaterThanEqualQuery<TKey> GreaterThanEqual<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.GreaterThanEqual<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static IInsideQuery<TKey> Inside<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.Inside<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), lowerKey, upperKey);
        }

        public static ILessThanQuery<TKey> LessThan<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.LessThan<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static ILessThanEqualQuery<TKey> LessThanEqual<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.LessThanEqual<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static INotAnyOfQuery<TKey> NotAllOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.NotAllOf<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), set);
        }

        public static INotAnyOfQuery<TKey> NotAnyOf<TGraph, TKey>(this IIndex<TGraph, TKey> index, IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.NotAnyOf<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), set);
        }

        public static INotEqualToQuery<TKey> NotEqualTo<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.NotEqualTo<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), key);
        }

        public static IIntersectOperator Or(this IQuery lhs, IQuery rhs)
        {
            return Kernel.Registry.BackingStore.Query.Or(lhs, rhs);
        }

        public static IOutsideQuery<TKey> Outside<TGraph, TKey>(this IIndex<TGraph, TKey> index, TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.Outside<TGraph, TKey>(getRegisteredIndexer<TGraph, TKey>(), lowerKey, upperKey);
        }

        private static IRegisteredIndexer getRegisteredIndexer<TGraph, TKey>() where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.GetRegistrationFor<TGraph>().GetRegisteredIndexerFor<IIndex<TGraph, TKey>>();
        }
    }
}