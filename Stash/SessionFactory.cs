namespace Stash
{
    using System;
    using System.Collections.Generic;
    using Configuration;

    public class SessionFactory
    {
        private readonly RegisteredStash registeredStash;

        public SessionFactory(RegisteredStash registeredStash)
        {
            this.registeredStash = registeredStash;
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