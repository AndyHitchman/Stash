namespace Stash.In.BDB
{
    using System;
    using System.Collections.Generic;
    using BerkeleyDB;

    public interface IBerkeleyBackingStoreParams
    {
        string DatabaseDirectory { get; }
        DatabaseEnvironmentConfig DatabaseEnvironmentConfig { get; }
        HashDatabaseConfig ValueDatabaseConfig { get; }
        Dictionary<Type, BTreeDatabaseConfig> IndexDatabaseConfigForTypes { get; }
    }
}