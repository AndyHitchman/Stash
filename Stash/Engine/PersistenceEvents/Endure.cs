namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.IO;

    public class Endure<TGraph> : Track<TGraph>
    {
        /// <summary>
        /// We are persisting a new graph here, so create a new <see cref="Guid"/>.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="session"></param>
        public Endure(TGraph graph, InternalSession session)
            : base(Guid.NewGuid(), graph, new MemoryStream(), session)
        {
        }


        public override void Complete()
        {
            new Insert<TGraph>(this).EnrollInSession();
        }
    }
}