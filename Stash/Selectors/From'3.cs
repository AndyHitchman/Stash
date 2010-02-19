namespace Stash.Selectors
{
    public abstract class From<TFromThis, TKey, TProjection> : From<TFromThis, TProjection> where TFromThis : From<TFromThis, TKey, TProjection>
    {
        protected From(IProjectedIndex<TKey> projector)
        {
            Projector = projector;
        }

        protected IProjectedIndex<TKey> Projector { get; private set; }


        protected TFromThis FromThis()
        {
            return (TFromThis)this;
        }
    }
}