namespace Stash.Configuration
{
    using Engine;

    /// <summary>
    /// A configured indexer.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class RegisteredIndexer<TGraph,TKey> : RegisteredIndexer<TGraph>
    {
        public RegisteredIndexer(Indexer<TGraph,TKey> indexer)
        {
            Indexer = indexer;
        }

        /// <summary>
        /// The mapper.
        /// </summary>
        public virtual Indexer<TGraph,TKey> Indexer { get; private set; }

        public override void EngageBackingStore(BackingStore backingStore)
        {
            backingStore.EnsureIndexer(Indexer);
        }
    }
}