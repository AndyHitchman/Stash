namespace Stash.Queries
{
    using System;

    public interface IOpenRangeQuery<TKey> : IQuery where TKey : IComparable<TKey>
    {
        TKey Key { get; }
    }
}