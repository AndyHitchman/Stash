namespace Stash.Engine.PersistenceEvents
{
    public class Destroy<TGraph> : CommonPersistenceEvent<TGraph>
    {
        public Destroy(TGraph graph) : base(graph)
        {
        }
    }
}