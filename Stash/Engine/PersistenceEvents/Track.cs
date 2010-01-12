namespace Stash.Engine.PersistenceEvents
{
    using System;
    using Configuration;

    public class Track<TGraph> : CommonPersistenceEvent<TGraph>
    {
        public Track(TGraph graph, InternalSession session) : base(graph, session)
        {
        }

        public override void Complete()
        {
            throw new NotImplementedException();
        }

        public override void EnrollInSession()
        {
            //Calculate indexes, maps and reduces on tracked graphs. This should allow any changes to be determined by comparison,
            //saving unecessary work in the backing store.
            //Keep a local cache of indexes, maps and reduces for graphs tracked in the session. Go here before hitting the
            //backing store. Reduces may be out of date once retrieved.
            //Exclude destroyed graphs from results.
            //Reduces must be calculated by a background process to ensure consistency. Use a BDB Queue?

            if (Session.GraphIsTracked(Graph))
                return;

            InstructSessionToEnrollThis();

            var registeredGraph = Session.Registry.GetGraphFor<TGraph>();
            calculateIndexes(registeredGraph);
            calculateMaps(registeredGraph);
        }

        public override void FlushFromSession()
        {
            throw new NotImplementedException();
        }

        private void calculateIndexes(RegisteredGraph<TGraph> registeredGraph)
        {
        }

        private void calculateMaps(RegisteredGraph<TGraph> registeredGraph)
        {
        }
    }
}