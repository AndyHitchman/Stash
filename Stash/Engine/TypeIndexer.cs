namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class TypeIndexer : Indexer<object,Type>
    {
        public Func<object, IEnumerable<Projection<Type, object>>> Index()
        {
            return o => GetTypeHierarchyFor(o);
        }

        protected virtual IEnumerable<Projection<Type,object>> GetTypeHierarchyFor(object o)
        {
            var t = o.GetType();

            do
            {
                yield return new Projection<Type,object>(t, o);
                t = t.BaseType;             
            } 
            while(t != null && t != typeof(object));
        }
    }
}