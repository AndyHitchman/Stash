namespace Stash.Engine.PersistenceEvents
{
    public class Endure<TGraph> : CommonPersistenceEvent<TGraph>
    {
        public Endure(TGraph graph) : base(graph)
        {
        }
    }
}