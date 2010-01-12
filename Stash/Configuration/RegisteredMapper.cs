namespace Stash.Configuration
{
    using Engine;

    /// <summary>
    /// A configured Map.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public abstract class RegisteredMapper<TGraph> where TGraph : class
    {
        public abstract void EngageBackingStore(BackingStore backingStore);
    }
}