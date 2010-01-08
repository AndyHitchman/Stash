namespace Stash.Configuration
{
    using Engine;

    public class RegisteredReducer<TKey, TValue>
    {
        public RegisteredReducer(Reduction<TKey, TValue> reduction)
        {
            Reduction = reduction;
        }

        public Reduction<TKey, TValue> Reduction { get; private set; }

        public virtual void EngageBackingStore(BackingStore backingStore)
        {
            backingStore.EnsureReduction(Reduction);
        }
    }
}