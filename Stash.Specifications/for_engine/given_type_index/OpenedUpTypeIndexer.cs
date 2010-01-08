namespace Stash.Specifications.for_engine.given_type_index
{
    using System;
    using System.Collections.Generic;
    using Engine;

    public class OpenedUpTypeIndex : TypeIndex
    {
        public IEnumerable<Projection<Type, object>> OpenedGetTypeHierarchyFor(object o)
        {
            return base.GetTypeHierarchyFor(o);
        }
    }
}