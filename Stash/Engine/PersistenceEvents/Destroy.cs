namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Destroy<TGraph> : PersistenceEvent<TGraph> where TGraph : class
    {
        public Destroy(Guid internalId, TGraph graph, InternalSession session)
        {
            InternalId = internalId;
            Graph = graph;
            Session = session;
        }

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
        public virtual object UntypedGraph
        {
            get { return Graph; }
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }

        public void FlushFromSession()
        {
            throw new NotImplementedException();
        }

        public PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(PersistenceEvent @event)
        {
            return PreviouslyEnrolledEvent.ShouldBeEvicted;
        }

        public void EnrollInSession()
        {
            Session.Enroll(this);
        }
    }
}