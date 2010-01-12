namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using PersistenceEvents;

    public class DefaultInternalSession : InternalSession
    {
        private readonly List<PersistenceEvent> enrolledPersistenceEvents;

        public DefaultInternalSession(Registry registry)
        {
            Registry = registry;
            InternalRepository = new DefaultUnenlistedRepository();
            enrolledPersistenceEvents = new List<PersistenceEvent>();
        }

        public UnenlistedRepository InternalRepository { get; private set; }

        public virtual Registry Registry { get; private set; }

        public virtual BackingStore BackingStore
        {
            get { return Registry.BackingStore; }
        }

        public IEnumerable<PersistenceEvent> EnrolledPersistenceEvents
        {
            get { return enrolledPersistenceEvents; }
        }

        public IEnumerable<object> TrackedGraphs
        {
            get { return enrolledPersistenceEvents.Select(@event => @event.UntypedGraph); }
        }

        public virtual void Dispose()
        {
            End();
        }

        public virtual void End()
        {
            throw new NotImplementedException();
        }

        public virtual EnlistedRepository EnlistRepository(UnenlistedRepository unenlistedRepository)
        {
            return new DefaultEnlistedRepository(this, unenlistedRepository);
        }

        public bool GraphIsTracked(object graph)
        {
            return TrackedGraphs.Any(o => ReferenceEquals(o, graph));
        }

        public void Enroll(PersistenceEvent persistenceEvent)
        {
            enrolledPersistenceEvents.Add(persistenceEvent);
        }

        public virtual InternalSession Internalize()
        {
            return this;
        }
    }
}