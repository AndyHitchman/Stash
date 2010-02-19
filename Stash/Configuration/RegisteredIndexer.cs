namespace Stash.Configuration
{
    using System.Collections.Generic;
    using Engine;

    public abstract class RegisteredIndexer<TGraph>
    {
        public abstract void EngageBackingStore(IBackingStore backingStore);

        /// <summary>
        /// Calls the index function and returns a <see cref="IProjectedIndex"/>
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public abstract IEnumerable<IProjectedIndex> GetKeyFreeProjections(TGraph graph);
    }
}