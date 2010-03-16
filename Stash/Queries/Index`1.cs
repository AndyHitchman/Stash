namespace Stash.Queries
{
    using System;
    using System.Collections.Generic;

    public static class Index<TIndex> where TIndex : IIndex
    {
        public static IAllOfQuery<TKey> AllOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.AllOf(Kernel.Registry.GetIndexerFor<TIndex>(), set);
        }

        public static IAnyOfQuery<TKey> AnyOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.AnyOf(Kernel.Registry.GetIndexerFor<TIndex>(), set);
        }

        public static IBetweenQuery<TKey> Between<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.Between(Kernel.Registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        public static IEqualToQuery<TKey> EqualTo<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.EqualTo(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static IGreaterThanQuery<TKey> GreaterThan<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.GreaterThan(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static IGreaterThanEqualQuery<TKey> GreaterThanEqual<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.GreaterThanEqual(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static IInsideQuery<TKey> Inside<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.Inside(Kernel.Registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }

        public static IIsIndexedQuery IsIndexed<TKey>()
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.IsIndexed(Kernel.Registry.GetIndexerFor<TIndex>());
        }

        public static ILessThanQuery<TKey> LessThan<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.LessThan(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static ILessThanEqualQuery<TKey> LessThanEqual<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.LessThanEqual(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static INotAnyOfQuery<TKey> NotAllOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.NotAllOf(Kernel.Registry.GetIndexerFor<TIndex>(), set);
        }

        public static INotAnyOfQuery<TKey> NotAnyOf<TKey>(IEnumerable<TKey> set)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.NotAnyOf(Kernel.Registry.GetIndexerFor<TIndex>(), set);
        }

        public static INotEqualToQuery<TKey> NotEqualTo<TKey>(TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.NotEqualTo(Kernel.Registry.GetIndexerFor<TIndex>(), key);
        }

        public static IOutsideQuery<TKey> Outside<TKey>(TKey lowerKey, TKey upperKey)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return Kernel.Registry.BackingStore.Query.Outside(Kernel.Registry.GetIndexerFor<TIndex>(), lowerKey, upperKey);
        }
    }
}