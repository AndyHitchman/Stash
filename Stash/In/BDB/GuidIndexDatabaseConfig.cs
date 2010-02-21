namespace Stash.In.BDB
{
    public class GuidIndexDatabaseConfig : IndexDatabaseConfig
    {
        public GuidIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsGuid().CompareTo(dbt2.Data.AsGuid());
        }
    }
}