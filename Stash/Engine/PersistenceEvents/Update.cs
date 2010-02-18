namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Update<TGraph> : PersistenceEvent<TGraph>
    {
        private readonly Track<TGraph> trackEvent;

        public Update(Track<TGraph> trackEvent)
        {
            this.trackEvent = trackEvent;
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }

        public object UntypedGraph
        {
            get { return trackEvent.UntypedGraph; }
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
            get { return trackEvent.Graph; }
        }

        public InternalSession Session
        {
            get { return trackEvent.Session; }
        }
    }
}