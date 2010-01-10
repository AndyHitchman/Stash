namespace Stash
{
    using System;
    using System.Collections.Generic;

    public interface Map<TGraph,TKey,TValue> : Map<TGraph>, Projector<TKey, TGraph>
    {
        /// <summary>
        /// The map function that projects the persisted <typeparam name="TGraph">object graph</typeparam> to 
        /// a <typeparam name="TKey">key</typeparam> and <typeparam name="TValue">value</typeparam>.
        /// </summary>
        Func<TGraph, IEnumerable<Projection<TKey, TValue>>> Map();
    }
}