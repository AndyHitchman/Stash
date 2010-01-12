namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using PersistenceEvents;

    public class DefaultInternalSession : InternalSession
    {
        private readonly List<PersistenceEvent> enrolledPersistentsEvents;

        public DefaultInternalSession(Registry registry)
        {
            Registry = registry;
            InternalRepository = new DefaultUnenlistedRepository();
            enrolledPersistentsEvents = new List<PersistenceEvent>();
        }

        public DefaultUnenlistedRepository InternalRepository { get; private set; }

        public virtual Registry Registry { get; private set; }

        public virtual BackingStore BackingStore
        {
            get { return Registry.BackingStore; }
        }

        public IEnumerable<PersistenceEvent> EnrolledPersistenceEvents
        {
            get { return enrolledPersistentsEvents; }
        }

        public IEnumerable<object> TrackedGraphs
        {
            get { return enrolledPersistentsEvents.Select(@event => @event.UntypedGraph); }
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

        public virtual void Enroll<TGraph>(PersistenceEvent<TGraph> persistenceEvent)
        {
            if (!Registry.IsManagingGraphTypeOrAncestor(typeof(TGraph)))
                throw new ArgumentOutOfRangeException("persistenceEvent", "The graph type is not being managed by Stash");

            persistenceEvent.SessionIs(this);
            persistenceEvent.EnrollInSession();
            //Calculate indexes, maps and reduces on tracked graphs. This should allow any changes to be determined by comparison,
            //saving unecessary work in the backing store.
            //Keep a local cache of indexes, maps and reduces for graphs tracked in the session. Go here before hitting the
            //backing store. Reduces may be out of date once retrieved.
            //Exclude destroyed graphs from results.
            //Reduces must be calculated by a background process to ensure consistency. Use a BDB Queue?

            throw new NotImplementedException();
        }

        public virtual InternalSession Internalize()
        {
            return this;
        }
    }
}