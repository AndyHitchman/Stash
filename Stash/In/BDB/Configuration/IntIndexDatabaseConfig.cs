namespace Stash.In.BDB.Configuration
{
    using System;

    public class IntIndexDatabaseConfig : IndexDatabaseConfig
    {
        public IntIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsInt().CompareTo(dbt2.Data.AsInt());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((int)key).AsByteArray();
        }
    }
}