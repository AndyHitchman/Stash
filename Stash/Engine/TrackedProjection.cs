namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class TrackedProjection<TValue>
    {
        public TrackedProjection(IEnumerable<Guid> originalIds, Projection<TValue> projection)
        {
            OriginalIds = originalIds;
            Projection = projection;
        }

        public virtual IEnumerable<Guid> OriginalIds { get; private set; }
        public virtual Projection<TValue> Projection { get; private set; }
    }
}