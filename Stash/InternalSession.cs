namespace Stash
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;

    public interface InternalSession : Session
    {
        /// <summary>
        /// The registered configuration.
        /// </summary>
        Registration Registration { get; }

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