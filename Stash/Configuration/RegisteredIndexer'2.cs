namespace Stash.Configuration
{
    using System;
    using Engine;

    /// <summary>
    /// A configured indexer.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class RegisteredIndexer<TGraph, TKey> : RegisteredIndexer<TGraph>
    {
        public RegisteredIndexer(Indexer<TGraph, TKey> indexer)
        {
            Indexer = indexer;
        }

        /// <summary>
        /// The indexer.
        /// </summary>
        public virtual Indexer<TGraph, TKey> Indexer { get; private set; }

        /// <summary>
        /// The indexer, without the <typeparam name="TKey"/>.
        /// </summary>
        public override Indexer<TGraph> KeylessIndexer
        {
            get { return Indexer; }
        }

        public override void EngageBackingStore(BackingStore backingStore)
        {
            backingStore.EnsureIndexer(Indexer);
        }
    }
}