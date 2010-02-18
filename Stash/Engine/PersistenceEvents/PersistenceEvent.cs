namespace Stash.Engine.PersistenceEvents
{
    /// <summary>
    /// An event that must be handled by the active <see cref="Session"/>.
    /// </summary>
    public interface PersistenceEvent
    {
        /// <summary>
        /// Enroll the persistence event in the session.
        /// </summary>
        void EnrollInSession();

        /// <summary>
        /// Complete all work for the persistence event.
        /// </summary>
        void Complete();

        /// <summary>
        /// Get the untypes graph.
        /// </summary>
        object UntypedGraph { get; }

        PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(PersistenceEvent @event);
    }
}