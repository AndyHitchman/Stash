namespace Stash.Configuration
{
    using System;
    using Engine;

    public class RegisteredReducer
    {
        public RegisteredReducer(Reducer reducer)
        {
            Reducer = reducer;
        }

        public Reducer Reducer { get; private set; }

        public virtual void EngageBackingStore(BackingStore backingStore)
        {
            backingStore.EnsureReducer(Reducer);
        }
    }
}