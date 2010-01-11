namespace Stash.Engine
{
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

        /// <summary>
        /// Ensure that the given <paramref name="persistenceEvent"/> is managed.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="persistenceEvent"></param>
        void Enroll<TGraph>(PersistenceEvent<TGraph> persistenceEvent);
    }
}