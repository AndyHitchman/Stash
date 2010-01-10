namespace Stash.Selectors
{
    public class FirstFrom<TKey, TProjection> : From<TKey, TProjection>
    {
        public FirstFrom(Projector<TKey, TProjection> projector) : base(projector)
        {
        }

        public FirstFrom<TKey, TProjection> WithKey(TKey equalTo)
        {
            return this;
        }
    }
}