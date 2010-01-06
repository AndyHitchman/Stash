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

        public Registrar(TBackingStore backingStore)
        {
            this.backingStore = backingStore;
        }

        public virtual void ApplyConfiguration(RegisteredStash registeredStash)
        {
            registeredStash.EngageBackingStore(backingStore);
        }

        /// <summary>
        /// Configure Stash in the required <paramref name="persistenceConfigurationActions"/>. <see cref="Stash.ConfigurePersistence{TBackingStore}"/> is a static wrapper for this.
        /// </summary>
        /// <param name="persistenceConfigurationActions"></param>
        public virtual void ConfigurePersistence(Action<PersistenceContext<TBackingStore>> persistenceConfigurationActions)
        {
            var persistenceContext = new PersistenceContext<TBackingStore>(new RegisteredStash());
            persistenceConfigurationActions(persistenceContext);
            ApplyConfiguration(persistenceContext.RegisteredStash);
        }
    }
}