namespace Stash.In.BDB.Configuration
{
    using System;

    public class FloatIndexDatabaseConfig : IndexDatabaseConfig
    {
        public FloatIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsFloat().CompareTo(dbt2.Data.AsFloat());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((float)key).AsByteArray();
        }
    }
}