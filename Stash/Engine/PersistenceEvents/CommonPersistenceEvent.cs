namespace Stash.Engine.PersistenceEvents
{
    using System;

    public abstract class CommonPersistenceEvent<TGraph> : PersistenceEvent<TGraph>
    {
        protected CommonPersistenceEvent(Guid internalId, TGraph graph, InternalSession session)
        {
            InternalId = internalId;
            Graph = graph;
            Session = session;
        }

        public Guid InternalId { get; set; }

        /// <summary>
        /// The typed graph.
        /// </summary>
        public virtual TGraph Graph { get; private set; }

        /// <summary>
        /// The internal session to which the persistence event belongs.
        /// </summary>
        public virtual InternalSession Session { get; private set; }

        /// <summary>
        /// Get the untypes graph.
        /// </summary>
        public object UntypedGraph
        {
            get { return Graph; }
        }

        /// <summary>
        /// Complete all work for the persistence event.
        /// </summary>
        public abstract void Complete();

        /// <summary>
        /// Enroll the persistence event in the session.
        /// </summary>
        public abstract void EnrollInSession();

        /// <summary>
        /// Remove the persistence event from the session.
        /// </summary>
        public abstract void FlushFromSession();

        public void InstructSessionToEnrollThis()
        {
            Session.Enroll(this);
        }

        public abstract PreviouslyEnrolledEvent TellSessionWhatToDoWithPreviouslyEnrolledEvent(PersistenceEvent @event);

        public abstract void PrepareEnrollment();
    }
}