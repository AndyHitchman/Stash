namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.Linq;

    public class Track<TGraph> : CommonPersistenceEvent<TGraph>
    {
        public Track(TGraph graph) : base(graph)
        {
        }

        public override void EnrollInSession()
        {
            if (InternalSession.TrackedGraphs.Any(o => ReferenceEquals(o, Graph)))
                return;


        }

        public override void Complete()
        {
            throw new NotImplementedException();
        }

        public override void FlushFromSession()
        {
            throw new NotImplementedException();
        }
    }
}