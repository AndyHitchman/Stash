namespace Stash.Configuration
{
    using Engine;

    /// <summary>
    /// A configured Map.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public abstract class RegisteredMapper<TGraph>
    {
        public abstract void EngageBackingStore(BackingStore backingStore);
    }
}