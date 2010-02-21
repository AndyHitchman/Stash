namespace Stash.In.BDB
{
    public class ShortIndexDatabaseConfig : IndexDatabaseConfig
    {
        public ShortIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsShort().CompareTo(dbt2.Data.AsShort());
        }
    }
}