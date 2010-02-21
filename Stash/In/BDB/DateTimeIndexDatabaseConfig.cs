namespace Stash.In.BDB
{
    public class DateTimeIndexDatabaseConfig : IndexDatabaseConfig
    {
        public DateTimeIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsDateTime().CompareTo(dbt2.Data.AsDateTime());
        }
    }
}