namespace Stash.Engine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Configuration;
    using PersistenceEvents;

    public class DefaultInternalSession : InternalSession
    {
        protected readonly List<PersistenceEvent> enrolledPersistenceEvents;
        private readonly ReaderWriterLockSlim enrolledPersistenceEventsLocker = new ReaderWriterLockSlim();

        public DefaultInternalSession(Registry registry, PersistenceEventFactory persistenceEventFactory)
        {
            Registry = registry;
            PersistenceEventFactory = persistenceEventFactory;
            InternalRepository = new DefaultUnenlistedRepository();
            enrolledPersistenceEvents = new List<PersistenceEvent>();
        }

        public UnenlistedRepository InternalRepository { get; private set; }

        public Registry Registry { get; private set; }
        public PersistenceEventFactory PersistenceEventFactory { get; set; }

        public IBackingStore BackingStore
        {
            get { return Registry.BackingStore; }
        }

        public virtual IEnumerable<PersistenceEvent> EnrolledPersistenceEvents
        {
            get
            {
                enrolledPersistenceEventsLocker.EnterReadLock();
                try
                {
                    return enrolledPersistenceEvents.ToList();
                }
                finally
                {
                    enrolledPersistenceEventsLocker.ExitReadLock();
                }
            }
        }

        public virtual IEnumerable<object> TrackedGraphs
        {
            get
            {
                enrolledPersistenceEventsLocker.EnterReadLock();
                try
                {
                    return enrolledPersistenceEvents.Select(_ => _.UntypedGraph).ToList();
                }
                finally
                {
                    enrolledPersistenceEventsLocker.ExitReadLock();
                }
            }
        }

        public void Abandon()
        {
            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                enrolledPersistenceEvents.Clear();
            }
            finally
            {
                enrolledPersistenceEventsLocker.ExitWriteLock();
            }
        }

        public virtual void Complete()
        {
            while(enrolledPersistenceEvents.Any())
            {
                phaseComplete();
            }
        }

        public virtual void Dispose()
        {
            Complete();
        }

        public virtual EnlistedRepository EnlistRepository(UnenlistedRepository unenlistedRepository)
        {
            return new DefaultEnlistedRepository(this, unenlistedRepository);
        }

        public void Enroll(PersistenceEvent persistenceEvent)
        {
            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                foreach(
                    var @event in
                        enrolledPersistenceEvents.Where(_ => ReferenceEquals(persistenceEvent.UntypedGraph, _.UntypedGraph)))
                {
                    //TODO: Act on answer (determine whether answer is ever useful.)
                    persistenceEvent.SayWhatToDoWithPreviouslyEnrolledEvent(@event);
                }
                enrolledPersistenceEvents.Add(persistenceEvent);
            }
            finally
            {
                enrolledPersistenceEventsLocker.ExitWriteLock();
            }
        }

        public bool GraphIsTracked(object graph)
        {
            return TrackedGraphs.Any(o => ReferenceEquals(o, graph));
        }

        public virtual InternalSession Internalize()
        {
            return this;
        }

        private void phaseComplete()
        {
            List<PersistenceEvent> drain;

            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                drain = enrolledPersistenceEvents.ToList();
                enrolledPersistenceEvents.Clear();
            }
            finally
            {
                enrolledPersistenceEventsLocker.ExitWriteLock();
            }

            foreach(var @event in drain)
            {
                @event.Complete();
            }
        }
    }
}