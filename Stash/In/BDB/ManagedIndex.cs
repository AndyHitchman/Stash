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
    using System.Collections;
    using BerkeleyConfigs;
    using BerkeleyDB;

    public class ManagedIndex
    {
        public ManagedIndex(string indexName, Type yieldsType, BTreeDatabase indexDatabase, HashDatabase reverseIndexDatabase, IndexDatabaseConfig indexDatabaseConfig)
        {
            Name = indexName;
            Index = indexDatabase;
            ReverseIndex = reverseIndexDatabase;
            config = indexDatabaseConfig;
            YieldsType = yieldsType;
        }

        public string Name { get; set; }
        public BTreeDatabase Index { get; private set; }
        public HashDatabase ReverseIndex { get; private set; }
        private IndexDatabaseConfig config;

        public Type YieldsType { get; private set; }

        public byte[] KeyAsByteArray(object key)
        {
            return config.KeyAsByteArray(key);
        }

        public IComparer Comparer { get { return config.GetComparer(); } }

        public object ByteArrayAsKey(byte[] bytes)
        {
            return config.ByteArrayAsKey(bytes);
        }

        public void Close()
        {
            Index.Close();
            ReverseIndex.Close();
        }
    }
}