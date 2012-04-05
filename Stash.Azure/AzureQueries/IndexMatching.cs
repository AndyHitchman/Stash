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

namespace Stash.Azure.AzureQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.StorageClient;
    using Stash.Azure;
    using Stash.Engine;

    public class IndexMatching
    {
        public static IEnumerable<InternalId> GetMatching<TKey>(ManagedIndex managedIndex, TableServiceContext serviceContext, TKey key) where TKey : IComparable<TKey>
        {
            return
                (from fi in managedIndex.Index(serviceContext)
                    where fi.PartitionKey == managedIndex.KeyAsString(key)
                    select managedIndex.ConvertToInternalId(fi.RowKey));
        }

        public static IEnumerable<TKey> GetReverseMatching<TKey>(ManagedIndex managedIndex, TableServiceContext serviceContext, InternalId internalId) where TKey : IComparable<TKey>
        {
            return
                (from ri in managedIndex.ReverseIndex(serviceContext)
                 where ri.PartitionKey == internalId.ToString()
                 select (TKey)managedIndex.ConvertToKey(ri.RowKey));
        }
    }
}