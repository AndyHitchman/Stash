namespace Stash.Selectors
{
    public abstract class From<TKey, TProjection> : From<TProjection>
    {
        protected From(Projector<TKey, TProjection> projector)
        {
            Projector = projector;
        }

        protected Projector<TKey, TProjection> Projector { get; private set; }
    }
}