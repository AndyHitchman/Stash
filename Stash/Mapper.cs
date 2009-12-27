namespace Stash
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implement to map for a <typeparamref name="TGraph"/>.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public interface Mapper<TGraph> : Projector
    {
        /// <summary>
        /// The map function that projects the persisted <typeparam name="TGraph">object graph</typeparam> to 
        /// a <typeparam name="TKey">key</typeparam> and <typeparam name="TValue">value</typeparam>.
        /// </summary>
        Func<TGraph, IEnumerable<Projection<TKey, TValue>>> Map<TKey, TValue>();
    }
}