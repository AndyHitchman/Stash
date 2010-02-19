using System;
using System.Collections.Generic;

namespace Stash.Engine
{
    public interface IStoredGraph
    {
        Guid InternalId { get; }
        IEnumerable<byte> SerialisedGraph { get; }
        Type ConcreteType { get; }
        IEnumerable<Type> SuperTypes { get; }
    }
}