namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;

    public class Track<TGraph> : PotentialPersistenceEvent<TGraph>
    {
        public Dictionary<RegisteredIndexer<TGraph>, List<TrackedProjection<TGraph>>> IndexProjections =
            new Dictionary<RegisteredIndexer<TGraph>, List<TrackedProjection<TGraph>>>();

        public Dictionary<RegisteredMapper<TGraph>, List<TrackedProjection<TGraph>>> MapProjections =
            new Dictionary<RegisteredMapper<TGraph>, List<TrackedProjection<TGraph>>>();

        public Track(Guid internalId, TGraph graph, InternalSession session) : base(internalId, graph, session)
        {
        }

        public override void Complete()
        {
            throw new NotImplementedException();
        }

        public override void EnrollInSession()
        {
            //Keep a local cache of indexes, maps and reduces for graphs tracked in the session. Go here before hitting the
            //backing store. Reduces may be out of date once retrieved.
            //Exclude destroyed graphs from results.
            //Reduces must be calculated by a background process to ensure consistency. Use a BDB Queue?

            if(Session.GraphIsTracked(Graph))
                return;

            InstructSessionToEnrollThis();
            PrepareEnrollment();
        }

        public override void FlushFromSession()
        {
            throw new NotImplementedException();
        }

        public override void PrepareEnrollment()
        {
            var registeredGraph = Session.Registry.GetRegistrationFor<TGraph>();

            //Calculate indexes, maps and reduces on tracked graphs. This should allow any changes to be determined by comparison,
            //saving unecessary work in the backing store.
            calculateIndexes(registeredGraph);
            calculateMaps(registeredGraph);
        }

        public override PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(PersistenceEvent @event)
        {
            return PreviouslyEnrolledEvent.ShouldBeRetained;
        }

        private void calculateIndexes(RegisteredGraph<TGraph> registeredGraph)
        {
            foreach(var registeredIndexer in registeredGraph.RegisteredIndexers)
            {
                IndexProjections.Add(
                    registeredIndexer,
                    registeredIndexer.GetKeyFreeProjections(Graph)
                        .Select(projection => new TrackedProjection<TGraph>(new[] {InternalId}, projection))
                        .ToList());
            }
        }

        private void calculateMaps(RegisteredGraph<TGraph> registeredGraph)
        {
            foreach(var registeredMapper in registeredGraph.RegisteredMappers)
            {
                MapProjections.Add(
                    registeredMapper,
                    registeredMapper.GetKeyFreeProjections(Graph)
                        .Select(projection => new TrackedProjection<TGraph>(new[] {InternalId}, projection))
                        .ToList());
            }
        }
    }
}