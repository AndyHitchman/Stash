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

        /// <summary>
        /// Configure Stash in the required <paramref name="persistenceConfigurationActions"/>. <see cref="Stash.ConfigurePersistence{TBackingStore}"/> is a static wrapper for this.
        /// </summary>
        /// <param name="persistenceConfigurationActions"></param>
        public virtual void ConfigurePersistence(Action<PersistenceContext<TBackingStore>> persistenceConfigurationActions)
        {
            var persistenceContext = new PersistenceContext<TBackingStore>();
            persistenceConfigurationActions(persistenceContext);
            ApplyConfiguration(persistenceContext);
        }

        public virtual void ApplyConfiguration(PersistenceContext<TBackingStore> persistenceContext)
        {
            EngageBackingStore(persistenceContext);
        }

        /// <summary>
        /// Engage the backing store in managing the persistence context.
        /// </summary>
        public void EngageBackingStore(PersistenceContext<TBackingStore> context)
        {
            foreach (var registeredGraph in context.AllRegisteredGraphs)
            {
                registeredGraph.EngageBackingStore(backingStore);
            }
        }
    }
}