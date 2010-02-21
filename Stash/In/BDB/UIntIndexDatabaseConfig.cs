namespace Stash.In.BDB
{
    public class UIntIndexDatabaseConfig : IndexDatabaseConfig
    {
        public UIntIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsUInt().CompareTo(dbt2.Data.AsUInt());
        }
    }
}