namespace Stash.In.BDB.Configuration
{
    using System;

    public class ULongIndexDatabaseConfig : IndexDatabaseConfig
    {
        public ULongIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsULong().CompareTo(dbt2.Data.AsULong());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((ulong)key).AsByteArray();
        }
    }
}