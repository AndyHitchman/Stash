namespace Stash.Configuration
{
    using System;

    /// <summary>
    /// The starting point for configuring Stash.
    /// </summary>
    public class ConfigurationEngine<TBackingStore> where TBackingStore : BackingStore
    {
        private PersistenceContext context;

        /// <summary>
        /// Configure Stash in the required <paramref name="persistenceConfigurationActions"/>. <see cref="Stash.Configure"/> is a static wrapper for this.
        /// </summary>
        /// <param name="persistenceConfigurationActions"></param>
        public void Persist(Action<PersistenceContext> persistenceConfigurationActions)
        {
            context = new PersistenceContext();
            persistenceConfigurationActions(context);
        }

        public void ApplyConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}