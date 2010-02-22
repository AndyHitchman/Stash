namespace Stash.In.BDB.Configuration
{
    using System;

    public class ShortIndexDatabaseConfig : IndexDatabaseConfig
    {
        public ShortIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsShort().CompareTo(dbt2.Data.AsShort());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((short)key).AsByteArray();
        }
    }
}