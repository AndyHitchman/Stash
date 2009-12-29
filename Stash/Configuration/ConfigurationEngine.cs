namespace Stash.Configuration
{
    using System;

    /// <summary>
    /// The starting point for configuring Stash.
    /// </summary>
    public class ConfigurationEngine<TBackingStore> where TBackingStore : BackingStore
    {
        private readonly PersistenceContext persistenceContext;
        private readonly TBackingStore backingStore;

        public ConfigurationEngine(TBackingStore backingStore)
        {
            this.backingStore = backingStore;
            persistenceContext = new PersistenceContext();
        }

        /// <summary>
        /// Configure Stash in the required <paramref name="persistenceConfigurationActions"/>. <see cref="Stash.ConfigurePersistence{TBackingStore}"/> is a static wrapper for this.
        /// </summary>
        /// <param name="persistenceConfigurationActions"></param>
        public void ConfigurePersistence(Action<PersistenceContext> persistenceConfigurationActions)
        {
            persistenceConfigurationActions(persistenceContext);
            ApplyConfiguration();
        }

        public void ApplyConfiguration()
        {
        }
    }
}