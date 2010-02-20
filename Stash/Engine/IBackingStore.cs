namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public interface IBackingStore
    {
        void InTransactionDo(Action<IStorageWork> storageWorkActions);

        IStoredGraph Get(Guid internalId);

        void EnsureIndex(string indexName, Type yieldsType);
    }
}