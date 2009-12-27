namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class ReduceProjector
    {
        /// <summary>
        /// Requires a function that accepts a <see cref="Projection{TKey,TValue}"> aggregate and <see cref="Projection{TKey,TValue}"> 
        /// instance and returns an accumulated aggregate <see cref="Projection{TKey,TValue}">projections</see> of the same shape.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="func"></param>
        TrackedProjection<TKey,TValue> Emit<TKey, TValue>(Guid originalId, TKey key, TValue accumulator, TValue instance, Func<TKey, TValue, TValue> func)
        {
            throw new NotImplementedException();
        }
    }
}