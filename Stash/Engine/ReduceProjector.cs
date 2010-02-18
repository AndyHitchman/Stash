namespace Stash.Engine
{
    using System;

    public class ReduceProjector
    {
        /// <summary>
        /// Requires a function that accepts a <see cref="Projection{TKey,TValue}"> aggregate and <see cref="Projection{TKey,TValue}"> 
        /// instance and returns an accumulated aggregate <see cref="Projection{TKey,TValue}">projections</see> of the same shape.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="instance"></param>
        /// <param name="func"></param>
        /// <param name="originalId"></param>
        /// <param name="key"></param>
        /// <param name="accumulator"></param>
        public virtual TrackedProjection Emit<TKey, TValue>(
            Guid originalId, TKey key, TValue accumulator, TValue instance, Func<TKey, TValue, TValue> func)
        {
            throw new NotImplementedException();
        }
    }
}