namespace Stash
{
    public abstract class Projection
    {
        public abstract object UntypedKey { get; }
        public abstract object UntypedValue { get; }
    }
}