namespace Stash.Engine.PersistenceEvents
{
    public class CommonPersistenceEvent<TGraph> : PersistenceEvent<TGraph>
    {
        protected CommonPersistenceEvent(TGraph graph)
        {
            Graph = graph;
        }

        protected virtual TGraph Graph { get; set; }

        protected virtual InternalSession InternalSession { get; set; }

        public virtual void EnlistedSessionIs(InternalSession internalSession)
        {
            InternalSession = internalSession;
        }
    }
}