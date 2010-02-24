namespace Stash.Configuration
{
    using System;

    public interface IRegisteredIndexer
    {
        Type IndexType { get; }
        string IndexName { get; }
        Type YieldType { get; }
    }
}