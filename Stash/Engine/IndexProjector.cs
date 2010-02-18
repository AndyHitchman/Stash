namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class IndexProjector
    {
        /// <summary>
        /// Requires a function that accepts a persisted object graph and yields zero, one 
        /// or many <see cref="Projection{TKey,TGraph}">projections</see>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="func"></param>
        public virtual IEnumerable<TrackedProjection> Emit<TGraph, TKey>(
            Func<Guid, TGraph, IEnumerable<Projection<TKey, TGraph>>> func)
        {
            throw new NotImplementedException();
        }
    }
}