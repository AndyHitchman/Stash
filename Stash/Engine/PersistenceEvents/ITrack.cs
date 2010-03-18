namespace Stash.Engine.PersistenceEvents
{
    using System;

    public interface ITrack<TSuperGraph> : IPersistenceEvent
    {
        /// <summary>
        /// The hash code calculated from the serialised graph at the time this track is created.
        /// </summary>
        byte[] OriginalHash { get; }

        /// <summary>
        /// The hash code calculated from the serialised graph at the time this track is completed.
        /// </summary>
        byte[] CompletionHash { get; }

        Guid InternalId { get; set; }

        TSuperGraph Graph { get; }
    }
}