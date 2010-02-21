namespace Stash.In.BDB
{
    public class IntIndexDatabaseConfig : IndexDatabaseConfig
    {
        public IntIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsInt().CompareTo(dbt2.Data.AsInt());
        }
    }
}