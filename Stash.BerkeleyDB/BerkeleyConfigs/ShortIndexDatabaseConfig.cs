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
    using System.Collections.Generic;

    public class ShortIndexDatabaseConfig : IndexDatabaseConfig
    {
        public ShortIndexDatabaseConfig()
        {
            BTreeCompare = (dbt1, dbt2) => dbt1.Data.AsShort().CompareTo(dbt2.Data.AsShort());
        }

        public override object ByteArrayAsKey(byte[] bytes)
        {
            return bytes.AsShort();
        }

        public override IComparer GetComparer()
        {
            return Comparer<short>.Default;
        }

        public override byte[] KeyAsByteArray(object key)
        {
            return ((short)key).AsByteArray();
        }
    }
}