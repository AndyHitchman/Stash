namespace Stash.Queries
{
    using System;

    public interface IClosedRangeQuery<TKey> : IQuery where TKey : IComparable<TKey>
    {
        TKey LowerKey { get; }
        TKey UpperKey { get; }
    }
}