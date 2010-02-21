namespace Stash.In.BDB
{
    public class DoubleIndexDatabaseConfig : IndexDatabaseConfig
    {
        public DoubleIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsDouble().CompareTo(dbt2.Data.AsDouble());
        }
    }
}