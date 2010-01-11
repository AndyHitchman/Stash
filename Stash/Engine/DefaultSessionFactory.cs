namespace Stash.Engine
{
    using System;
    using Configuration;

    public class DefaultSessionFactory : SessionFactory
    {
        private readonly Registration registration;

        public DefaultSessionFactory(Registration registration)
        {
            this.registration = registration;
        }


        /// <summary>
        /// Get a session.
        /// </summary>
        /// <returns></returns>
        public Session GetSession()
        {
            return new DefaultInternalSession(registration);
        }
    }
}