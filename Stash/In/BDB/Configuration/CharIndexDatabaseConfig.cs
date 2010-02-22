namespace Stash.In.BDB.Configuration
{
    using System;

    public class CharIndexDatabaseConfig : IndexDatabaseConfig
    {
        public CharIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsChar().CompareTo(dbt2.Data.AsChar());
        }

        public override byte[] AsByteArray(object key)
        {
            return ((char)key).AsByteArray();
        }
    }
}