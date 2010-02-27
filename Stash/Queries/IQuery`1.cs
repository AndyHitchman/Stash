namespace Stash.Queries
{
    using System;
    using Configuration;

    public interface IQuery<TKey> : IQuery where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        IRegisteredIndexer Indexer { get; }
    }
}