namespace Stash.In.BDB
{
    public class FloatIndexDatabaseConfig : IndexDatabaseConfig
    {
        public FloatIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsFloat().CompareTo(dbt2.Data.AsFloat());
        }
    }
}