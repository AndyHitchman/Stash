namespace Stash.Engine.PersistenceEvents
{
    /// <summary>
    /// An event that must be handled by the active <see cref="Session"/>.
    /// </summary>
    public interface PersistenceEvent
    {
        /// <summary>
        /// Get a handle for the session.
        /// </summary>
        /// <param name="internalSession"></param>
        void SessionIs(InternalSession internalSession);

        /// <summary>
        /// Enroll the persistence event in the session.
        /// </summary>
        void EnrollInSession();

        /// <summary>
        /// Complete all work for the persistence event.
        /// </summary>
        void Complete();

        /// <summary>
        /// Remove the persistence event from the session.
        /// </summary>
        void FlushFromSession();

        /// <summary>
        /// Get the untypes graph.
        /// </summary>
        object UntypedGraph { get; }
    }
}