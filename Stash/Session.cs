namespace Stash
{
    using System;
    using Engine;

    public interface Session : IDisposable
    {
        /// <summary>
        /// Tell the session to track the work performed with the given repository.
        /// </summary>
        /// <param name="unenlistedRepository"></param>
        EnlistedRepository EnlistRepository(UnenlistedRepository unenlistedRepository);

        /// <summary>
        /// Get the <see cref="InternalSession"/> used by Stash. Not for external use.
        /// </summary>
        /// <returns></returns>
        InternalSession Internalize();

        /// <summary>
        /// End the session.
        /// </summary>
        void End();
    }
}