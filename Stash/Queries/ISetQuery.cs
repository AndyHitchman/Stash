namespace Stash.Queries
{
    using System;
    using System.Collections.Generic;

    public interface ISetQuery<TKey> : IQuery where TKey : IEquatable<TKey>
    {
        IEnumerable<TKey> Set { get; }
    }
}