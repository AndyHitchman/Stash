namespace Stash.Configuration
{
    using System.Collections.Generic;
    using Engine;

    public abstract class RegisteredIndexer<TGraph>
    {
        public abstract void EngageBackingStore(BackingStore backingStore);

        /// <summary>
        /// Calls the index function and returns a <see cref="Projection{TGraph}"/>
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public abstract IEnumerable<Projection<TGraph>> GetKeyFreeProjections(TGraph graph);
    }
}