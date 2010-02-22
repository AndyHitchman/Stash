namespace Stash.In.BDB.Configuration
{
    using System;

    public class UShortIndexDatabaseConfig : IndexDatabaseConfig
    {
        public UShortIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsUShort().CompareTo(dbt2.Data.AsUShort());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((ushort)key).AsByteArray();
        }
    }
}