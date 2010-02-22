namespace Stash.In.BDB.Configuration
{
    using System;

    public class BooleanIndexDatabaseConfig : IndexDatabaseConfig
    {
        public BooleanIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsBoolean().CompareTo(dbt2.Data.AsBoolean());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((bool)key).AsByteArray();
        }
    }
}