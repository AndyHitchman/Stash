namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummyMap : Map<DummyPersistentObject,object,object>
    {
        public IEnumerable<Projection<object, object>> F(DummyPersistentObject graph)
        {
            throw new NotImplementedException();
        }
    }
}