namespace Stash
{
    using System;
    using System.Collections.Generic;
    using Configuration;

    public class ActualSessionFactory : SessionFactory
    {
        private readonly Registration registration;

        public ActualSessionFactory(Registration registration)
        {
            this.registration = registration;
        }


        /// <summary>
        /// Get a session.
        /// </summary>
        /// <returns></returns>
        public Session GetSession()
        {
            throw new NotImplementedException(); 
        }
    }
}