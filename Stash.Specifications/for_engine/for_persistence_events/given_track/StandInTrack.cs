namespace Stash.Specifications.for_engine.for_persistence_events.given_track
{
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Engine.PersistenceEvents;

    public class StandInTrack<TGraph> : Track<TGraph> where TGraph : class
    {
        public bool HasCalculatedIndexes;

        public StandInTrack(IStoredGraph storedGraph, IRegisteredGraph<TGraph> registeredGraph)
            : base(storedGraph, registeredGraph) {}

        protected override IEnumerable<IProjectedIndex> CalculateIndexes()
        {
            HasCalculatedIndexes = true;
            return Enumerable.Empty<IProjectedIndex>();
        }
    }
}