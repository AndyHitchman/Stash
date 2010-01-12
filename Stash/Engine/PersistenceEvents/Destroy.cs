namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Destroy<TGraph> : CommonPersistenceEvent<TGraph> where TGraph : class
    {
        public Destroy(TGraph graph) : base(graph)
        {
        }

        public override void EnrollInSession()
        {
            throw new NotImplementedException();
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