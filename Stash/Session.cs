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
        Action<UnenlistedRepository> EnlistRepository(UnenlistedRepository unenlistedRepository);

        /// <summary>
        /// Get the <see cref="InternalSession"/> used by Stash. Not for external use.
        /// </summary>
        /// <returns></returns>
        InternalSession Internalize();

        /// <summary>
        /// Push all enrolled changes to the <see cref="BackingStore"/>.
        /// </summary>
        void Flush();

        /// <summary>
        /// End the session. Implementations must call <see cref="Flush"/>.
        /// </summary>
        void End();
    }
}