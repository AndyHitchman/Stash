namespace Stash.In.BDB
{
    public class UShortIndexDatabaseConfig : IndexDatabaseConfig
    {
        public UShortIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsUShort().CompareTo(dbt2.Data.AsUShort());
        }
    }
}