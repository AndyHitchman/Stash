namespace Stash
{
    using System;
    using System.Collections.Generic;
    using Engine;

    public interface Session : IDisposable
    {
        /// <summary>
        /// Tell the session to track the work performed with the given repository.
        /// </summary>
        /// <param name="unenlistedRepository"></param>
        Action<UnenlistedRepository> EnlistRepository(UnenlistedRepository unenlistedRepository);

        
    }
}