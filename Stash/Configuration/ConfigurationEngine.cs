namespace Stash.Configuration
{
    using System;
    using Engine;

    /// <summary>
    /// The starting point for configuring Stash.
    /// </summary>
    public class ConfigurationEngine<TBackingStore> where TBackingStore : BackingStore
    {
        private readonly TBackingStore backingStore;

        public ConfigurationEngine(TBackingStore backingStore)
        {
            this.backingStore = backingStore;
        }

        /// <summary>
        /// Configure Stash in the required <paramref name="persistenceConfigurationActions"/>. <see cref="Stash.ConfigurePersistence{TBackingStore}"/> is a static wrapper for this.
        /// </summary>
        /// <param name="persistenceConfigurationActions"></param>
        public void ConfigurePersistence(Action<PersistenceContext> persistenceConfigurationActions)
        {
            var persistenceContext = new PersistenceContext();
            persistenceConfigurationActions(persistenceContext);
            ApplyConfiguration(persistenceContext);
        }

        public void ApplyConfiguration(PersistenceContext persistenceContext)
        {
            foreach(var registeredGraph in persistenceContext.AllRegisteredGraphs)
            {
//                registeredGraph.
            }
        }
    }
}