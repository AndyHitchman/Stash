namespace Stash.In.BDB.Configuration
{
    using System;

    public class DateTimeIndexDatabaseConfig : IndexDatabaseConfig
    {
        public DateTimeIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsDateTime().CompareTo(dbt2.Data.AsDateTime());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((DateTime)key).AsByteArray();
        }
    }
}