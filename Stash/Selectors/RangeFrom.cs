namespace Stash.Selectors
{
    public class RangeFrom<TKey, TProjection> : From<RangeFrom<TKey, TProjection>, TKey, TProjection>
    {
        public RangeFrom(Projector<TKey, TProjection> projector) : base(projector)
        {
        }

        public RangeFrom<TKey, TProjection> Between(TKey greaterThanOrEqualTo, TKey lessThanOrEqualTo)
        {
            return FromThis();
        }
    }
}