namespace Stash.Selectors
{
    public abstract class From<TFromThis, TKey, TProjection> : From<TFromThis, TProjection> where TFromThis : From<TFromThis, TKey, TProjection>
    {
        protected From(Projector<TKey, TProjection> projector)
        {
            Projector = projector;
        }

        protected Projector<TKey, TProjection> Projector { get; private set; }


        protected TFromThis FromThis()
        {
            return (TFromThis)this;
        }
    }
}