namespace Stash.In.BDB
{
    using System;
    using System.Collections.Generic;
    using BerkeleyDB;
    using Configuration;

    public class BerkeleyBackingStoreParams : IBerkeleyBackingStoreParams
    {
        public BerkeleyBackingStoreParams(
            string databaseDirectory,
            DatabaseEnvironmentConfig databaseEnvironmentConfig,
            HashDatabaseConfig valueDatabaseConfig,
            IndexDatabaseConfig objectIndexDatabaseConfig,
            BooleanIndexDatabaseConfig booleanIndexDatabaseConfig,
            CharIndexDatabaseConfig charIndexDatabaseConfig,
            DateTimeIndexDatabaseConfig dateTimeIndexDatabaseConfig,
            DoubleIndexDatabaseConfig doubleIndexDatabaseConfig,
            FloatIndexDatabaseConfig floatIndexDatabaseConfig,
            GuidIndexDatabaseConfig guidIndexDatabaseConfig,
            IntIndexDatabaseConfig intIndexDatabaseConfig,
            LongIndexDatabaseConfig longIndexDatabaseConfig,
            ShortIndexDatabaseConfig shortIndexDatabaseConfig,
            StringIndexDatabaseConfig stringIndexDatabaseConfig,
            TypeIndexDatabaseConfig typeIndexDatabaseConfig,
            UIntIndexDatabaseConfig uIntIndexDatabaseConfig,
            ULongIndexDatabaseConfig uLongIndexDatabaseConfig,
            UShortIndexDatabaseConfig uShortIndexDatabaseConfig)
        {
            DatabaseDirectory = databaseDirectory;
            DatabaseEnvironmentConfig = databaseEnvironmentConfig;
            ValueDatabaseConfig = valueDatabaseConfig;
            IndexDatabaseConfigForTypes = new Dictionary<Type, IndexDatabaseConfig>
                {
                    {typeof(object), objectIndexDatabaseConfig},
                    {typeof(bool), booleanIndexDatabaseConfig},
                    {typeof(char), charIndexDatabaseConfig},
                    {typeof(DateTime), dateTimeIndexDatabaseConfig},
                    {typeof(double), doubleIndexDatabaseConfig},
                    {typeof(float), floatIndexDatabaseConfig},
                    {typeof(Guid), guidIndexDatabaseConfig},
                    {typeof(int), intIndexDatabaseConfig},
                    {typeof(long), longIndexDatabaseConfig},
                    {typeof(short), shortIndexDatabaseConfig},
                    {typeof(string), stringIndexDatabaseConfig},
                    {typeof(Type), typeIndexDatabaseConfig},
                    {typeof(uint), uIntIndexDatabaseConfig},
                    {typeof(ulong), uLongIndexDatabaseConfig},
                    {typeof(ushort), uShortIndexDatabaseConfig},
                };
        }

        public string DatabaseDirectory { get; private set; }
        public DatabaseEnvironmentConfig DatabaseEnvironmentConfig { get; private set; }
        public HashDatabaseConfig ValueDatabaseConfig { get; private set; }
        public Dictionary<Type, IndexDatabaseConfig> IndexDatabaseConfigForTypes { get; private set; }
    }
}