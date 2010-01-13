namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Endure<TGraph> : Track<TGraph> where TGraph : class
    {
        public Endure(TGraph graph, InternalSession session) : base(Guid.NewGuid(), graph, session)
        {
        }

        public override void Complete()
        {
            throw new NotImplementedException();
        }
    }
}