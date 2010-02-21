namespace Stash.In.BDB
{
    public class ULongIndexDatabaseConfig : IndexDatabaseConfig
    {
        public ULongIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsULong().CompareTo(dbt2.Data.AsULong());
        }
    }
}