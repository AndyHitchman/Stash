namespace Stash.In.BDB.Configuration
{
    using System;

    public class GuidIndexDatabaseConfig : IndexDatabaseConfig
    {
        public GuidIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsGuid().CompareTo(dbt2.Data.AsGuid());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((Guid)key).AsByteArray();
        }
    }
}