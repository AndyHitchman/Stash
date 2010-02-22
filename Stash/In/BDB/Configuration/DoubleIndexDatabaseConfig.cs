namespace Stash.In.BDB.Configuration
{
    using System;

    public class DoubleIndexDatabaseConfig : IndexDatabaseConfig
    {
        public DoubleIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsDouble().CompareTo(dbt2.Data.AsDouble());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((double)key).AsByteArray();
        }
    }
}