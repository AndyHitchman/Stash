namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;

    public class TrackedProjection
    {
        public TrackedProjection(IEnumerable<Guid> originalIds, IProjectedIndex projection)
        {
            OriginalIds = originalIds;
            Projection = projection;
        }

        public virtual IEnumerable<Guid> OriginalIds { get; private set; }
        public virtual IProjectedIndex Projection { get; private set; }
    }
}