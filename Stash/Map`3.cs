﻿namespace Stash
{
    using System;
    using System.Collections.Generic;

    public interface Map<TGraph,TKey,TValue> : Map<TGraph>
    {
        /// <summary>
        /// The map function that projects the persisted <typeparam name="TGraph">object graph</typeparam> to 
        /// a <typeparam name="TKey">key</typeparam> and <typeparam name="TValue">value</typeparam>.
        /// </summary>
        IEnumerable<IProjectedIndex<TKey>> F(TGraph graph);
    }
}