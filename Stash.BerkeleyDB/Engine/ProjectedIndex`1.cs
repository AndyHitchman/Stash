namespace Stash.BerkeleyDB.Engine
{
    using System;
    using BackingStore;

    public class ProjectedIndex<TKey> : ProjectedIndex, IProjectedIndex<TKey>
    {
        public ProjectedIndex(string indexName, TKey key)
        {
            IndexName = indexName;
            Key = key;
        }

        public TKey Key { get; private set; }

        public override string IndexName { get; set; }

        public override Type TypeOfKey
        {
            get { return typeof(TKey); }
        }

        public override object UntypedKey
        {
            get { return Key; }
        }
    }
}