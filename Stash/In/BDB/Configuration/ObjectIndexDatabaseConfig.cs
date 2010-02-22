namespace Stash.In.BDB.Configuration
{
    using System;

    public class ObjectIndexDatabaseConfig : IndexDatabaseConfig
    {
        public ObjectIndexDatabaseConfig()
        {
        }

        public override byte[] AsByteArray(object key)
        {
            return ((object)key).ToString().AsByteArray();
        }
    }
}