namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.Linq;

    public class Endure<TGraph> : Track<TGraph> where TGraph : class
    {
        public Endure(TGraph graph) : base(graph)
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
    }
}