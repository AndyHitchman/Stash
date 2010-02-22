#region License

// Copyright 2009 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.

#endregion

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