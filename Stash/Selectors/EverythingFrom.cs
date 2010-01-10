namespace Stash.Selectors
{
    public class EverythingFrom<TKey, TProjection> : From<TKey, TProjection>
    {
        public EverythingFrom(Projector<TKey, TProjection> projector) : base(projector)
        {
        }

        public EverythingFrom<TKey, TProjection> WithKey(TKey equalTo)
        {
            return this;
        }
    }
}