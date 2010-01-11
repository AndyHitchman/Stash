namespace Stash.Engine
{
    using System;
    using Configuration;

    public class DefaultSessionFactory : SessionFactory
    {
        private readonly Registry registry;

        public DefaultSessionFactory(Registry registry)
        {
            this.registry = registry;
        }


        /// <summary>
        /// Get a session.
        /// </summary>
        /// <returns></returns>
        public Session GetSession()
        {
            return new DefaultInternalSession(registry);
        }
    }
}