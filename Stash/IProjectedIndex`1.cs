namespace Stash
{
    public interface IProjectedIndex<TKey> : IProjectedIndex
    {
        TKey Key { get; }
    }
}