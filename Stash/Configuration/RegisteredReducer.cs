namespace Stash.Configuration
{
    using Engine;

    public class RegisteredReducer
    {
        public RegisteredReducer(Reduction reduction)
        {
            Reduction = reduction;
        }

        public Reduction Reduction { get; private set; }

        public virtual void EngageBackingStore(BackingStore backingStore)
        {
            backingStore.EnsureReduction(Reduction);
        }
    }
}