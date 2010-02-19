namespace Stash.Selectors
{
    public class FirstFrom<TKey, TProjection> : From<FirstFrom<TKey,TProjection> ,TKey, TProjection>
    {
        public FirstFrom(IProjectedIndex<TKey> projector) : base(projector)
        {
        }

        public FirstFrom<TKey, TProjection> WithKey(TKey equalTo)
        {
            return FromThis();
        }
    }
}