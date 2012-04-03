namespace Stash.Azure
{
    using System;
    using BackingStore;

    public abstract class ProjectedIndex : IProjectedIndex 
    {
        public abstract string IndexName { get; protected set; }
        public abstract Type TypeOfKey { get; }
        public abstract object UntypedKey { get; }
    }
}