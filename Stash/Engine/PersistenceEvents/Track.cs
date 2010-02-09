namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using Configuration;

    public class Track<TGraph> : PersistenceEvent<TGraph>
    {
        protected readonly Dictionary<RegisteredIndexer<TGraph>, List<TrackedProjection<TGraph>>> IndexProjections =
            new Dictionary<RegisteredIndexer<TGraph>, List<TrackedProjection<TGraph>>>();

        protected readonly Dictionary<RegisteredMapper<TGraph>, List<TrackedProjection<TGraph>>> MapProjections =
            new Dictionary<RegisteredMapper<TGraph>, List<TrackedProjection<TGraph>>>();

        private readonly SHA1CryptoServiceProvider hashCodeGenerator;

        public Track(Guid internalId, TGraph graph, Stream serializedGraph, InternalSession session)
        {
            InternalId = internalId;
            Graph = graph;
            Session = session;
            hashCodeGenerator = new SHA1CryptoServiceProvider();
            OriginalHash = hashCodeGenerator.ComputeHash(serializedGraph);
        }

        /// <summary>
        /// The hash code calculated from the serialised graph at the time this track is created.
        /// </summary>
        public byte[] OriginalHash { get; private set; }

        public Guid InternalId { get; set; }

        /// <summary>
        /// The typed graph.
        /// </summary>
        public TGraph Graph { get; private set; }

        /// <summary>
        /// The internal session to which the persistence event belongs.
        /// </summary>
        public InternalSession Session { get; private set; }

        /// <summary>
        /// Get the untypes graph.
        /// </summary>
        public object UntypedGraph
        {
            get { return Graph; }
        }

        public virtual void Complete()
        {
            var serializedGraph = Session.Registry.Serializer().Serialize(Graph);
            var currentHash = hashCodeGenerator.ComputeHash(serializedGraph);
            
            if(currentHash.SequenceEqual(OriginalHash))
                //No change to object. No work to do.
                return;

            var registeredGraph = Session.Registry.GetRegistrationFor<TGraph>();

            //Calculate indexes, maps and reduces on tracked graphs. This should allow any changes to be determined by comparison,
            //saving unecessary work in the backing store.
            CalculateIndexes(registeredGraph);
            CalculateMaps(registeredGraph);

            Session.PersistenceEventFactory.MakeUpdate(this).EnrollInSession();
        }

        public virtual void EnrollInSession()
        {
            //Keep a local cache of indexes, maps and reduces for graphs tracked in the session. Go here before hitting the
            //backing store. Reduces may be out of date once retrieved.
            //Exclude destroyed graphs from results.
            //Reduces must be calculated by a background process to ensure consistency. Use a BDB Queue?

            if(Session.GraphIsTracked(Graph))
                return;

            Session.Enroll(this);
            PrepareEnrollment();
        }

        public virtual void FlushFromSession()
        {
            throw new NotImplementedException();
        }

        public virtual void PrepareEnrollment()
        {
        }

        public virtual PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(PersistenceEvent @event)
        {
            return PreviouslyEnrolledEvent.ShouldBeRetained;
        }

        protected virtual void CalculateIndexes(RegisteredGraph<TGraph> registeredGraph)
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

        protected virtual void CalculateMaps(RegisteredGraph<TGraph> registeredGraph)
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