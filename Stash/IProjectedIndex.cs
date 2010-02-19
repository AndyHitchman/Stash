namespace Stash
{
    using System;

    public interface IProjectedIndex
    {
        string IndexName { get; }
        Type TypeOfKey { get; }
        object UntypedKey { get; }
    }
}