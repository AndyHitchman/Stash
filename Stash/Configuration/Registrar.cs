namespace Stash.Configuration
{
    using System;
    using Engine;

    /// <summary>
    /// The starting point for configuring Stash.
    /// </summary>
    public class Registrar<TBackingStore> where TBackingStore : BackingStore
    {
        private readonly TBackingStore backingStore;
        private PersistenceContext<TBackingStore> persistenceContext;

        public Registrar(TBackingStore backingStore)
        {
            this.backingStore = backingStore;
        }

        public virtual Registry Registry
        {
            get { return persistenceContext.Registry; }
        }

        public virtual void ApplyRegistration()
        {
            persistenceContext.Registry.EngageBackingStore(backingStore);
        }

        /// <summary>
        /// Configure Stash in the required <paramref name="persistenceConfigurationActions"/>.
        /// </summary>
        /// <param name="persistenceConfigurationActions"></param>
        public virtual void PerformRegistration(Action<PersistenceContext<TBackingStore>> persistenceConfigurationActions)
        {
            persistenceContext = new PersistenceContext<TBackingStore>(new Registry());
            persistenceConfigurationActions(persistenceContext);
        }
    }
}