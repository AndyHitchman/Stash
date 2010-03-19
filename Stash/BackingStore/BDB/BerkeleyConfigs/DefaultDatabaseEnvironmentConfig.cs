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

namespace Stash.BackingStore.BDB.BerkeleyConfigs
{
    using BerkeleyDB;

    public class DefaultDatabaseEnvironmentConfig : DatabaseEnvironmentConfig
    {
        public DefaultDatabaseEnvironmentConfig()
        {
            const string dataDir = "data";

            AutoCommit = false;
            Create = true;
            ErrorPrefix = "stash";
            UseLogging = true;
            UseTxns = true;
            FreeThreaded = true;
            RunRecovery = true;

            UseMPool = true;
            MPoolSystemCfg = new MPoolConfig {CacheSize = new CacheInfo(0, 128 * 1024, 1)};

            UseLocking = true;
            LockSystemCfg = new LockingConfig {DeadlockResolution = DeadlockPolicy.MIN_WRITE};

            CreationDir = dataDir;
            DataDirs.Add(dataDir);
        }
    }
}