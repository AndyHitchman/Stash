namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class TrackedProjection
    {
        public TrackedProjection(IEnumerable<Guid> originalIds, Projection projection)
        {
            OriginalIds = originalIds;
            Projection = projection;
        }

        public virtual IEnumerable<Guid> OriginalIds { get; private set; }
        public virtual Projection Projection { get; private set; }
    }
}