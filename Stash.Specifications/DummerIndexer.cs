namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummerIndexer : Indexer<DummyPersistentObject>
    {
        #region Indexer<DummyPersistentObject> Members

        public Func<DummyPersistentObject, IEnumerable<Projection<TKey, DummyPersistentObject>>> Index<TKey>()
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