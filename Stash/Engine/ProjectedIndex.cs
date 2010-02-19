namespace Stash.Engine
{
    using System;

    public class ProjectedIndex<TKey> : IProjectedIndex<TKey>
    {
        public ProjectedIndex(string indexName, TKey key)
        {
            IndexName = indexName;
            Key = key;
        }

        public TKey Key { get; private set; }

        public string IndexName { get; private set; }

        public Type TypeOfKey
        {
            get { return typeof(TKey); }
        }

        public object UntypedKey
        {
            get { return Key; }
        }
    }
}