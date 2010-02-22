namespace Stash.In.BDB.Configuration
{
    using System;

    public class TypeIndexDatabaseConfig : IndexDatabaseConfig
    {
        public TypeIndexDatabaseConfig()
        {
        }

        public override byte[] AsByteArray(object key)
        {
            return ((Type)key).AsByteArray();
        }
    }
}