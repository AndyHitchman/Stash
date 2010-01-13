namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class TypeIndex : Index<object, Type>
    {
        protected virtual IEnumerable<Projection<Type, object>> GetTypeHierarchyFor(object o)
        {
            var t = o.GetType();

            do
            {
                yield return new Projection<Type, object>(t, o);
                t = t.BaseType;
            } while(t != null && t != typeof(object));
        }

        public Func<object, IEnumerable<Projection<Type, object>>> F()
        {
            return o => GetTypeHierarchyFor(o);
        }
    }
}