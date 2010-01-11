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

        public virtual Action<UnenlistedRepository> EnlistRepository(UnenlistedRepository unenlistedRepository)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}