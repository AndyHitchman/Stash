namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class TypeIndex : Index<object,Type>
    {
        protected virtual IEnumerable<Type> GetTypeHierarchyFor(object o)
        {
            var t = o.GetType();

            do
            {
                yield return t;
                t = t.BaseType;
            } while(t != null && t != typeof(object));
        }

        public IEnumerable<Type> Yield(object graph)
        {
            return GetTypeHierarchyFor(graph);
        }
    }
}