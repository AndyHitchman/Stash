namespace Stash
{
    using System.Collections.Generic;

    /// <summary>
    /// Implement an index for a tracked graph.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface Index<TGraph, TKey>
    {
        /// <summary>
        /// The index function that accepts the persisted object and yields a set of <typeparamref name="TKey"/>"/>
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        IEnumerable<TKey> Yield(TGraph graph);
    }
}