namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummyMapper : Mapper<DummyPersistentObject>
    {
        #region Mapper<DummyPersistentObject> Members

        public Func<DummyPersistentObject, IEnumerable<Projection<TKey, TValue>>> Map<TKey, TValue>()
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}