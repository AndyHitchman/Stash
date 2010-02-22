namespace Stash.In.BDB
{
    using System;
    using System.Linq;
    using BerkeleyDB;
    using Configuration;

    public class IndexManager
    {
        public IndexManager(string indexName, Type yieldsType, BTreeDatabase indexDatabase, IndexDatabaseConfig indexDatabaseConfig)
        {
            IndexName = indexName;
            IndexDatabase = indexDatabase;
            IndexDatabaseConfig = indexDatabaseConfig;
            YieldsType = yieldsType;
        }

        public string IndexName { get; set; }
        public BTreeDatabase IndexDatabase { get; private set; }
        public IndexDatabaseConfig IndexDatabaseConfig { get; set; }
        public Type YieldsType { get; private set; }

        public void Close()
        {
            IndexDatabase.Close();
        }
    }
}