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
        public abstract void EngageBackingStore(BackingStore backingStore);

        /// <summary>
        /// Calls the index function and returns a <see cref="Projection{TGraph}"/>
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public abstract IEnumerable<Projection<TGraph>> GetKeyFreeProjections(TGraph graph);
    }
}