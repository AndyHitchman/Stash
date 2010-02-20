namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;
    using Engine;

    public class DummyBackingStore : IBackingStore
    {
        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            throw new NotImplementedException();
        }

        public IStoredGraph Get(Guid internalId)
        {
            throw new NotImplementedException();
        }

        public void EnsureIndex(string indexName, Type yieldsType)
        {
            throw new NotImplementedException();
        }
    }
}