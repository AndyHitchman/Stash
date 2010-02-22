namespace Stash.In.BDB.Configuration
{
    using System;

    public class TimeSpanIndexDatabaseConfig : IndexDatabaseConfig
    {
        public TimeSpanIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsTimeSpan().CompareTo(dbt2.Data.AsTimeSpan());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((TimeSpan)key).AsByteArray();
        }
    }
}