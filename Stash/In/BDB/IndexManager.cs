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
    using BerkeleyDB;
    using Configuration;

    public class IndexManager
    {
        public IndexManager(string indexName, Type yieldsType, BTreeDatabase indexDatabase, HashDatabase reverseIndexDatabase, IndexDatabaseConfig indexDatabaseConfig)
        {
            IndexName = indexName;
            IndexDatabase = indexDatabase;
            ReverseIndexDatabase = reverseIndexDatabase;
            IndexDatabaseConfig = indexDatabaseConfig;
            YieldsType = yieldsType;
        }

        public string IndexName { get; set; }
        public BTreeDatabase IndexDatabase { get; private set; }
        public HashDatabase ReverseIndexDatabase { get; private set; }
        public IndexDatabaseConfig IndexDatabaseConfig { get; private set; }
        public Type YieldsType { get; private set; }

        public void Close()
        {
            IndexDatabase.Close();
            ReverseIndexDatabase.Close();
        }
    }
}