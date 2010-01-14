namespace Stash
{
    using System;

    /// <summary>
    /// Implement to Reduction.
    /// </summary>
    public interface Reduction<TKey, TValue> : Projector<TKey, TValue>
    {
        /// <summary>
        /// The reduce function that aggregates <paramref name="value">values</paramref> for a given <paramref name="key"/>
        /// </summary>
        TValue Reduce(TKey key, TValue value);
    }
}