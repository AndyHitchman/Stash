namespace Stash.In.BDB.Configuration
{
    using System;

    public class LongIndexDatabaseConfig : IndexDatabaseConfig
    {
        public LongIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsLong().CompareTo(dbt2.Data.AsLong());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((long)key).AsByteArray();
        }
    }
}