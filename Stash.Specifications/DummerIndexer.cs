namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummerIndexer : Indexer<DummyPersistentObject>
    {
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public Func<DummyPersistentObject, IEnumerable<Projection<TKey, DummyPersistentObject>>> Index<TKey>()
        {
            throw new NotImplementedException();
        }
    }
}