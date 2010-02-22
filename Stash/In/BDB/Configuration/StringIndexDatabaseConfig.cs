namespace Stash.In.BDB.Configuration
{
    using System;

    public class StringIndexDatabaseConfig : IndexDatabaseConfig
    {
        public StringIndexDatabaseConfig()
        {
        }

        public override byte[] AsByteArray(object key)
        {
            return ((string)key).AsByteArray();
        }
    }
}