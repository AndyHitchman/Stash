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
        BackingStore BackingStore { get; }

        DefaultUnenlistedRepository InternalRepository { get; }
        IEnumerable<PersistenceEvent> EnrolledPersistenceEvents { get; }
        IEnumerable<object> TrackedGraphs { get; }

        /// <summary>
        /// Ensure that the given <paramref name="persistenceEvent"/> is managed.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="persistenceEvent"></param>
        void Enroll<TGraph>(PersistenceEvent<TGraph> persistenceEvent);
    }
}