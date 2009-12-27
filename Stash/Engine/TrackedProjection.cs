namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains a <see cref="Projection"/> that remains linked to the original persisted objects.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class TrackedProjection<TKey, TValue>
    {
        public TrackedProjection(IEnumerable<Guid> originalIds, Projection<TKey, TValue> projection)
        {
            OriginalIds = originalIds;
            Projection = projection;
        }

        public IEnumerable<Guid> OriginalIds { get; private set; }
        public Projection<TKey, TValue> Projection { get; private set; }
    }
}