namespace Stash.Selectors
{
    public class EverythingFrom<TKey, TProjection> : From<EverythingFrom<TKey, TProjection>, TKey, TProjection>
    {
        public EverythingFrom(IProjectedIndex<TKey> projector) : base(projector)
        {
        }

        public EverythingFrom<TKey, TProjection> WithKey(TKey equalTo)
        {
            return FromThis();
        }
    }
}