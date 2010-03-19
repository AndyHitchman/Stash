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

    public static class Index<TIndex> where TIndex : IIndex
    {
        public static IAllOfQuery<TKey> AllOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.AllOf(Kernel.Registry.GetIndexerFor<TIndex>(), set);
        }

        public static IAnyOfQuery<TKey> AnyOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.AnyOf(Kernel.Registry.GetIndexerFor<TIndex>(), set);
        }

        public static IBetweenQuery<TKey> Between<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.Between(Kernel.Registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        public static IEqualToQuery<TKey> EqualTo<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.EqualTo(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static IGreaterThanQuery<TKey> GreaterThan<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.GreaterThan(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static IGreaterThanEqualQuery<TKey> GreaterThanEqual<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.GreaterThanEqual(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static IInsideQuery<TKey> Inside<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.Inside(Kernel.Registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        public static IIsIndexedQuery IsIndexed<TKey>()
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.IsIndexed(Kernel.Registry.GetIndexerFor<TIndex>());
        }

        public static ILessThanQuery<TKey> LessThan<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.LessThan(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static ILessThanEqualQuery<TKey> LessThanEqual<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.LessThanEqual(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static INotAnyOfQuery<TKey> NotAllOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.NotAllOf(Kernel.Registry.GetIndexerFor<TIndex>(), set);
        }

        public static INotAnyOfQuery<TKey> NotAnyOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.NotAnyOf(Kernel.Registry.GetIndexerFor<TIndex>(), set);
        }

        public static INotEqualToQuery<TKey> NotEqualTo<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.NotEqualTo(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static IOutsideQuery<TKey> Outside<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.QueryFactory.Outside(Kernel.Registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }
    }
}