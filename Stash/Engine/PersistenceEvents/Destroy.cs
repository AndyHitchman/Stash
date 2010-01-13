namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Destroy<TGraph> : CommonPersistenceEvent<TGraph> where TGraph : class
    {
        public Destroy(Guid internalId, TGraph graph, InternalSession session) : base(internalId, graph, session)
        {
        }

        public override void Complete()
        {
            throw new NotImplementedException();
        }

        public override void FlushFromSession()
        {
            throw new NotImplementedException();
        }

        public override PreviouslyEnrolledEvent TellSessionWhatToDoWithPreviouslyEnrolledEvent(PersistenceEvent @event)
        {
            return PreviouslyEnrolledEvent.ShouldBeEvicted;
        }

        public override void PrepareEnrollment()
        {
            throw new NotImplementedException();
        }

        public override void EnrollInSession()
        {
            InstructSessionToEnrollThis();
        }
    }
}