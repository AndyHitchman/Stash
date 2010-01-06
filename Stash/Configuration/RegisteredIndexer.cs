namespace Stash.Configuration
{
    using Engine;

    public abstract class RegisteredIndexer<TGraph>
    {
        public abstract Indexer<TGraph> KeylessIndexer { get; }
        public abstract void EngageBackingStore(BackingStore backingStore);
    }
}