namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Endure<TGraph> : Track<TGraph> where TGraph : class
    {
        /// <summary>
        /// We are persisting a new graph here, so create a new <see cref="Guid"/>.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="session"></param>
        public Endure(TGraph graph, InternalSession session) : base(Guid.NewGuid(), graph, session)
        {
        }


        public override void Complete()
        {
            new Insert<TGraph>(this).EnrollInSession();
        }
    }
}