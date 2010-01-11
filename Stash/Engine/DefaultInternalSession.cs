namespace Stash.Engine
{
    using System;
    using Configuration;
    using PersistenceEvents;

    public class DefaultInternalSession : InternalSession
    {
        public DefaultInternalSession(Registry registry)
        {
            Registry = registry;
        }

        public virtual void Dispose()
        {
            End();
        }

        public virtual EnlistedRepository EnlistRepository(UnenlistedRepository unenlistedRepository)
        {
            return new DefaultEnlistedRepository(this, unenlistedRepository);
        }

        public virtual InternalSession Internalize()
        {
            return this;
        }

        public virtual void End()
        {
            throw new NotImplementedException();
        }

        public virtual Registry Registry { get; private set; }

        public virtual BackingStore BackingStore
        {
            get { return Registry.BackingStore; }
        }

        public virtual void Enroll<TGraph>(PersistenceEvent<TGraph> persistenceEvent)
        {
            persistenceEvent.EnlistedSessionIs(this);

            //Calculate indexes, maps and reduces on tracked graphs. This should allow any changes to be determined by comparison,
            //saving unecessary work in the backing store.
            //Keep a local cache of indexes, maps and reduces for graphs tracked in the session. Go here before hitting the
            //backing store. Reduces may be out of date once retrieved.
            //Exclude destroyed graphs from results.
            //Reduces must be calculated by a background process to ensure consistency. Use a BDB Queue?

            throw new NotImplementedException();
        }
    }
}