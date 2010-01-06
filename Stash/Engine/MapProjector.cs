namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class MapProjector
    {
        /// <summary>
        /// Requires a function that accepts a persisted object graph and yields zero, one 
        /// or many <see cref="Projection{TKey,TValue}">projections</see>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="func"></param>
        public virtual IEnumerable<TrackedProjection<TKey, TValue>> Emit<TGraph, TKey, TValue>(
            Func<Guid, TGraph, IEnumerable<Projection<TKey, TValue>>> func)
        {
            throw new NotImplementedException();
        }
    }
}