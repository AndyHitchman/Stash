namespace Stash.Engine
{
    using System;
    using Configuration;
    using PersistenceEvents;

    public class DefaultInternalSession : InternalSession
    {
        public DefaultInternalSession(Registration registration)
        {
            Registration = registration;
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

        public virtual Registration Registration { get; private set; }

        public virtual BackingStore BackingStore
        {
            get { return Registration.BackingStore; }
        }

        public virtual void Enroll<TGraph>(PersistenceEvent<TGraph> persistenceEvent)
        {
            //Calculate indexes, maps and reduces on track. This should allow any changes to be determined by comparison,
            //saving unecessary work in the backing store.
            //Keep a local cache of indexes, maps and reduces for graphs tracked in the session. Go here before hitting the
            //backing store. Reduces may be out of date once retrieved.
            //Reduces must be calculated by a background process to ensure consistency. Use a BDB Queue?
            throw new NotImplementedException();
        }
    }
}