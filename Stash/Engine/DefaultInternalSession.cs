namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Configuration;
    using PersistenceEvents;

    public class DefaultInternalSession : InternalSession
    {
        private readonly ReaderWriterLockSlim enrolledPersistenceEventsLocker = new ReaderWriterLockSlim();
        protected readonly List<PersistenceEvent> enrolledPersistenceEvents;

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

        public IEnumerable<object> TrackedGraphs
        {
            get
            {
                enrolledPersistenceEventsLocker.EnterReadLock();
                try
                {
                    return enrolledPersistenceEvents.Select(@event => @event.UntypedGraph).ToList();
                }
                finally
                {
                    enrolledPersistenceEventsLocker.ExitReadLock();
                }
            }
        }

        public virtual void Dispose()
        {
            Complete();
        }

        public virtual void Complete()
        {
            phaseComplete();
            phaseComplete();
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
            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                foreach(var @event in enrolledPersistenceEvents.Where(@event => ReferenceEquals(persistenceEvent.UntypedGraph, @event.UntypedGraph)))
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

        public virtual InternalSession Internalize()
        {
            return this;
        }
    }
}