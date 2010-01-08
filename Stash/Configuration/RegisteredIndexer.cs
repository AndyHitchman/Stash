namespace Stash.Configuration
{
    using Engine;

    public abstract class RegisteredIndexer<TGraph>
    {
        public abstract void EngageBackingStore(BackingStore backingStore);
    }
}