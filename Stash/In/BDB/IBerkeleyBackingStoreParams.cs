namespace Stash.In.BDB
{
    using System;
    using System.Collections.Generic;
    using BerkeleyDB;
    using Configuration;

    public interface IBerkeleyBackingStoreParams
    {
        string DatabaseDirectory { get; }
        DatabaseEnvironmentConfig DatabaseEnvironmentConfig { get; }
        HashDatabaseConfig ValueDatabaseConfig { get; }
        Dictionary<Type, IndexDatabaseConfig> IndexDatabaseConfigForTypes { get; }
    }
}