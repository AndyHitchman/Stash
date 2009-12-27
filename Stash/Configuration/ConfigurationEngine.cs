namespace Stash.Configuration
{
    using System;

    /// <summary>
    /// The starting point for configuring Stash.
    /// </summary>
    public class ConfigurationEngine
    {
        private PersistenceContext context;

        /// <summary>
        /// Configure Stash in the required <paramref name="configurationAction"/>. <see cref="Stash.Configure"/> is a static wrapper for this.
        /// </summary>
        /// <param name="configurationAction"></param>
        public void Configure(Action<PersistenceContext> configurationAction)
        {
            context = new PersistenceContext();
            configurationAction(context);
        }

        public void ApplyConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}