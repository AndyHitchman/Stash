namespace Stash.Engine.PersistenceEvents
{
    public abstract class CommonPersistenceEvent<TGraph> : PersistenceEvent<TGraph>
    {
        protected CommonPersistenceEvent(TGraph graph)
        {
            Graph = graph;
        }

        public virtual TGraph Graph { get; private set; }

        public virtual InternalSession InternalSession { get; set; }

        public object UntypedGraph
        {
            get { return Graph; }
        }

        public abstract void Complete();
        public abstract void EnrollInSession();
        public abstract void FlushFromSession();

        public virtual void SessionIs(InternalSession internalSession)
        {
            InternalSession = internalSession;
        }
    }
}