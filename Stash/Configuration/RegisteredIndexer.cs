namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;
    using Engine;

    public abstract class RegisteredIndexer<TGraph>
    {
        public abstract void EngageBackingStore(BackingStore backingStore);
    }
}