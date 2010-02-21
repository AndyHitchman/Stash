namespace Stash.In.BDB
{
    public class LongIndexDatabaseConfig : IndexDatabaseConfig
    {
        public LongIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsLong().CompareTo(dbt2.Data.AsLong());
        }
    }
}