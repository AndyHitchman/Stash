namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummerIndex : Index<DummyPersistentObject, object>
    {
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public Func<DummyPersistentObject, IEnumerable<Projection<object, DummyPersistentObject>>> F()
        {
            throw new NotImplementedException();
        }
    }
}