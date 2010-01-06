namespace Stash
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implement an index for a <typeparamref name="TGraph"/>.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface Indexer<TGraph, TKey> : Indexer<TGraph>
    {
        /// <summary>
        /// The index function that accepts the persisted object and returns a <see cref="Projection{TKey,TGraph}"/>
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        Func<TGraph, IEnumerable<Projection<TKey, TGraph>>> Index();
    }
}