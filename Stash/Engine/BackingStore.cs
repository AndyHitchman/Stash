namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public interface BackingStore : IDisposable
    {
        void InsertGraph(
            Guid internalId,
            byte[] serializedGraph,
            IEnumerable<Type> types,
            IEnumerable<Index<>>
            )
    }
}