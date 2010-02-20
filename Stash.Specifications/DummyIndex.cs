namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummyIndex : Index<DummyPersistentObject, int>
    {

        public IEnumerable<int> Yield(DummyPersistentObject graph)
        {
            throw new NotImplementedException();
        }
    }
}