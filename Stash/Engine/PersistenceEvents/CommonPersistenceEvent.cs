namespace Stash.Engine.PersistenceEvents
{
    using System;

    public abstract class CommonPersistenceEvent<TGraph> : PersistenceEvent<TGraph>
    {
        protected CommonPersistenceEvent(TGraph graph, InternalSession session)
        {
            Graph = graph;
            Session = session;
        }

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

        public void InstructSessionToEntrollThis()
        {
            Session.Enroll(this);
        }
    }
}