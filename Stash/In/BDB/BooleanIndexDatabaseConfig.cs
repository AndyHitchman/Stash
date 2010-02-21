namespace Stash.In.BDB
{
    public class BooleanIndexDatabaseConfig : IndexDatabaseConfig
    {
        public BooleanIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsBoolean().CompareTo(dbt2.Data.AsBoolean());
        }
    }
}