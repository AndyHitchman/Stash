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

namespace Stash.BerkeleyDB.BerkeleyConfigs
{
    using System.Collections;
    using global::BerkeleyDB;

    /// <summary>
    /// We can't use secondary databases for our indexes because the .NET wrapper only supports
    /// key generation functions that return a single value.
    /// </summary>
    public abstract class IndexDatabaseConfig : BTreeDatabaseConfig
    {
        protected IndexDatabaseConfig()
        {
            Creation = CreatePolicy.IF_NEEDED;
            Duplicates = DuplicatesPolicy.SORTED;
            FreeThreaded = true;
            ReadUncommitted = true;
            AutoCommit = true;
        }

        public abstract object ByteArrayAsKey(byte[] bytes);
        public abstract IComparer GetComparer();
        public abstract byte[] KeyAsByteArray(object key);
    }
}