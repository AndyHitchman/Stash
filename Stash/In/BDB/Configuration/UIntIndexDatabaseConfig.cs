namespace Stash.In.BDB.Configuration
{
    using System;

    public class UIntIndexDatabaseConfig : IndexDatabaseConfig
    {
        public UIntIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsUInt().CompareTo(dbt2.Data.AsUInt());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((uint)key).AsByteArray();
        }
    }
}