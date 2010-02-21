namespace Stash.In.BDB
{
    public class CharIndexDatabaseConfig : IndexDatabaseConfig
    {
        public CharIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsChar().CompareTo(dbt2.Data.AsChar());
        }
    }
}