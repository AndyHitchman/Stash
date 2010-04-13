#region License
// Copyright 2009, 2010 Andrew Hitchman
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

    public class ValueDatabaseConfig : HashDatabaseConfig
    {
        public ValueDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.NONE;
            FreeThreaded = true;
            ReadUncommitted = true;
            AutoCommit = true;
            //            HashComparison = (dbt1, dbt2) => dbt1.Data.AsGuid().CompareTo(dbt2.Data.AsGuid());
        }
    }
}