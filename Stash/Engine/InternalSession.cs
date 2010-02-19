namespace Stash.Engine
{
    using System.Collections.Generic;
    using Configuration;
    using PersistenceEvents;

    public interface InternalSession : Session
    {
        /// <summary>
        /// The registered configuration.
        /// </summary>
        Registry Registry { get; }

        /// <summary>
        /// The engaged backing store.
        /// </summary>
        IBackingStore BackingStore { get; }

        /// <summary>
        /// An internal repository for use by the session.
        /// </summary>
        UnenlistedRepository InternalRepository { get; }

        /// <summary>
        /// Persistence events enrolled in the session.
        /// </summary>
        IEnumerable<PersistenceEvent> EnrolledPersistenceEvents { get; }

        /// <summary>
        /// Graphs tracked by the session.
        /// </summary>
        IEnumerable<object> TrackedGraphs { get; }

        PersistenceEventFactory PersistenceEventFactory { get; set; }

        /// <summary>
        /// True if the graph is being tracked by this session.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        bool GraphIsTracked(object graph);

        /// <summary>
        /// Manage the persistence event.
        /// </summary>
        /// <param name="persistenceEvent"></param>
        void Enroll(PersistenceEvent persistenceEvent);
    }
}