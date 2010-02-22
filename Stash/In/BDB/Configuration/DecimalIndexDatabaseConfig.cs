namespace Stash.In.BDB.Configuration
{
    using System;

    public class DecimalIndexDatabaseConfig : IndexDatabaseConfig
    {
        public DecimalIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsDecimal().CompareTo(dbt2.Data.AsDecimal());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((decimal)key).AsByteArray();
        }
    }
}