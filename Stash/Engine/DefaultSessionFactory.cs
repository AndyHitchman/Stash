namespace Stash.Engine
{
    using System;
    using Configuration;
    using PersistenceEvents;

    public class DefaultSessionFactory : SessionFactory
    {
        private readonly Registry registry;
        private readonly PersistenceEventFactory persistenceEventFactory;

        public DefaultSessionFactory(Registry registry, PersistenceEventFactory persistenceEventFactory)
        {
            this.registry = registry;
            this.persistenceEventFactory = persistenceEventFactory;
        }


        /// <summary>
        /// Get a session.
        /// </summary>
        /// <returns></returns>
        public Session GetSession()
        {
            return new DefaultInternalSession(registry, persistenceEventFactory);
        }
    }
}