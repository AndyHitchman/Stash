namespace Stash
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implement to reducer.
    /// </summary>
    public interface Reducer : Projector
    {
        /// <summary>
        /// The reduce function that aggregates <typeparam name="TValue">values</typeparam> for a given <typeparam name="TKey"/>.
        /// </summary>
        Func<TKey, TValue, TValue> Reduce<TKey, TValue>();
    }
}