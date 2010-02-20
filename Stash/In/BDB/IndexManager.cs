namespace Stash.In.BDB
{
    using System;
    using System.Linq;
    using BerkeleyDB;

    public class IndexManager
    {
        public IndexManager(string indexName, Type yieldsType, BTreeDatabase indexDatabase)
        {
            IndexName = indexName;
            IndexDatabase = indexDatabase;
            YieldsType = yieldsType;
        }

        public string IndexName { get; set; }
        public BTreeDatabase IndexDatabase { get; private set; }

        public Type YieldsType { get; private set; }

        public void Close()
        {
            IndexDatabase.Close();
        }
    }
}