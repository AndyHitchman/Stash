namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Insert<TGraph> : PersistenceEvent<TGraph> where TGraph : class
    {
        private readonly Endure<TGraph> endureEvent;

        public Insert(Endure<TGraph> endureEvent)
        {
            this.endureEvent = endureEvent;
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }

        public void FlushFromSession()
        {
            throw new NotImplementedException();
        }

        public object UntypedGraph
        {
            get { throw new NotImplementedException(); }
        }

        public PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(PersistenceEvent @event)
        {
            return PreviouslyEnrolledEvent.ShouldBeRetained;
        }

        public void EnrollInSession()
        {
            Session.Enroll(this);
        }

        public TGraph Graph
        {
            get { return endureEvent.Graph; }
        }

        public InternalSession Session
        {
            get { return endureEvent.Session; }
        }
    }
}