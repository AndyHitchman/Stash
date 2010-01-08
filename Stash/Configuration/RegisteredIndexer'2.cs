namespace Stash.Configuration
{
    using System;
    using Engine;

    /// <summary>
    /// A configured Index.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class RegisteredIndexer<TGraph, TKey> : RegisteredIndexer<TGraph>
    {
        public RegisteredIndexer(Index<TGraph, TKey> index)
        {
            Index = index;
        }

        /// <summary>
        /// The Index.
        /// </summary>
        public virtual Index<TGraph, TKey> Index { get; private set; }

        public override void EngageBackingStore(BackingStore backingStore)
        {
            backingStore.EnsureIndex(Index);
        }
    }
}