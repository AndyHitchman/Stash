namespace Stash.BackingStore.BDB
{
    using System;
    using Configuration;

    public class InternalProjectedIndex : IProjectedIndex
    {
        private readonly IRegisteredIndexer registeredIndexer;
        private readonly object key;

        public InternalProjectedIndex(IRegisteredIndexer registeredIndexer, object key)
        {
            this.registeredIndexer = registeredIndexer;
            this.key = key;
        }

        public IRegisteredIndexer Indexer
        {
            get { return registeredIndexer; }
        }

        public string IndexName
        {
            get { return registeredIndexer.IndexName; }
        }

        public Type TypeOfKey
        {
            get { return registeredIndexer.YieldType; }
        }

        public object UntypedKey
        {
            get { return key; }
        }
    }
}