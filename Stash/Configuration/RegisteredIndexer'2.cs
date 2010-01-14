namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Engine;

    /// <summary>
    /// A configured Index.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class RegisteredIndexer<TGraph, TKey> : RegisteredIndexer<TGraph> where TGraph : class
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

        public override IEnumerable<Projection<TGraph>> GetKeyFreeProjections(TGraph graph)
        {
            return Index.F(graph).Cast<Projection<TGraph>>();
        }
    }
}