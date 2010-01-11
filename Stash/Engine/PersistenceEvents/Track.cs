namespace Stash.Engine.PersistenceEvents
{
    public class Track<TGraph> : CommonPersistenceEvent<TGraph>
    {
        public Track(TGraph graph) : base(graph)
        {
        }
    }
}