namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using System.Collections.Generic;
    using Engine;
    using Engine.PersistenceEvents;

    internal class StandInDefaultInternalSession : DefaultInternalSession
    {
        public StandInDefaultInternalSession() : base(null)
        {
        }

        public List<PersistenceEvent> ExposedPersistenceEvents { get { return enrolledPersistenceEvents; } }
    }
}