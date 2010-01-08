namespace Stash.Engine.PersistenceEvents
{
    public class Destroy<TGraph> : PersistenceEvent<TGraph>
    {
        public Destroy(TGraph graph)
        {
        }
    }
}