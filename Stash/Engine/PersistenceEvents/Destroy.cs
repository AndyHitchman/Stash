namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Destroy<TGraph> : CommonPersistenceEvent<TGraph> where TGraph : class
    {
        public Destroy(TGraph graph, InternalSession session) : base(graph, session)
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

        public override void EnrollInSession()
        {
            InstructSessionToEntrollThis();
        }
    }
}