namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class TypeIndexer : Indexer<object, Type>
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

        public Func<object, IEnumerable<Projection<Type, object>>> Index()
        {
            return o => GetTypeHierarchyFor(o);
        }
    }
}