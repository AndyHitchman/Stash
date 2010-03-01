namespace Stash.Queries
{
    using System;

    public interface IValueQuery<TKey> : IQuery where TKey : IEquatable<TKey>
    {
        TKey Key { get; }
    }
}