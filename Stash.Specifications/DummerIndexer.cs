namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummerIndex : Index<DummyPersistentObject, object>
    {
        public IEnumerable<object> Yield(DummyPersistentObject graph)
        {
            throw new NotImplementedException();
        }
    }
}