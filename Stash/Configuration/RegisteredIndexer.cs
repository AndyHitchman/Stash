namespace Stash.Configuration
{
    using Engine;

    public abstract class RegisteredIndexer<TGraph> where TGraph : class
    {
        public abstract void EngageBackingStore(BackingStore backingStore);
    }
}