namespace Stash.Configuration
{
    using System.Collections.Generic;
    using Engine;

    /// <summary>
    /// A configured Map.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public abstract class RegisteredMapper<TGraph>
    {
        public abstract void EngageBackingStore(IBackingStore backingStore);

        /// <summary>
        /// Calls the index function and returns a <see cref="Projection"/>
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public abstract IEnumerable<IProjectedIndex> GetKeyFreeProjections(TGraph graph);
    }
}