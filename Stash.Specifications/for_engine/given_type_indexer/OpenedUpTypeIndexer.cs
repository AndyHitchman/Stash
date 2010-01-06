namespace Stash.Specifications.for_engine.given_type_indexer
{
    using System;
    using System.Collections.Generic;
    using Engine;

    public class OpenedUpTypeIndexer : TypeIndexer
    {
        public IEnumerable<Projection<Type, object>> OpenedGetTypeHierarchyFor(object o)
        {
            return base.GetTypeHierarchyFor(o);
        }
    }
}