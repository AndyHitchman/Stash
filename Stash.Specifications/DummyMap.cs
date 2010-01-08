namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummyMap : Map<DummyPersistentObject,object,object>
    {
        public Func<DummyPersistentObject, IEnumerable<Projection<object, object>>> Map()
        {
            throw new NotImplementedException();
        }
    }
}