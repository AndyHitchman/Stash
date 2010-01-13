namespace Stash
{
    public abstract class Projection<TValue>
    {
        public abstract object UntypedKey { get; }
    }
}