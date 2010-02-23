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

    public interface IBerkeleyBackingStoreEnvironment
    {
        string DatabaseDirectory { get; }
        DatabaseEnvironmentConfig DatabaseEnvironmentConfig { get; }
        HashDatabaseConfig ValueDatabaseConfig { get; }
        HashDatabaseConfig ReverseIndexDatabaseConfig { get; }
        Dictionary<Type, IndexDatabaseConfig> IndexDatabaseConfigForTypes { get; }
        DatabaseEnvironment Environment { get; }
        void Close();
    }
}