namespace Stash.Selectors
{
    public interface From<TFromThis, TProjection> where TFromThis : From<TFromThis, TProjection>
    {
    }
}