namespace Stash.Engine.PersistenceEvents
{
    public interface PersistenceEvent<TGraph> : PersistenceEvent
    {
        /// <summary>
        /// The typed graph.
        /// </summary>
        TGraph Graph { get; }

        /// <summary>
        /// The internal session to which the persistence event belongs.
        /// </summary>
        InternalSession Session { get; }
    }
}