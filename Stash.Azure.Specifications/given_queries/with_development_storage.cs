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

namespace Stash.Azure.Specifications.given_queries
{
    using System;
    using System.IO;
    using BackingStore;
    using Configuration;
    using Engine;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;
    using Stash.Engine;
    using Support;

    public abstract class with_development_storage : Specification
    {
        protected IRegistry Registry;
        protected AzureBackingStore Subject;

        protected override void WithContext()
        {
            CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient().DeleteTableIfExist(AzureBackingStore.ConcreteTypeTableName);
            CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient().DeleteTableIfExist("idxStashEngineStashTypeHierarchy");
            CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient().DeleteTableIfExist("rdxStashEngineStashTypeHierarchy");

            Subject = new AzureBackingStore(CloudStorageAccount.DevelopmentStorageAccount, new NoConcurrencyProtection());
            Registry = new Registry(Subject);
            var typeHierarchyIndexer = new RegisteredIndexer<Type, string>(new StashTypeHierarchy(), Registry);
            Registry.RegisteredIndexers.Add(typeHierarchyIndexer);

            Subject.EnsureIndex(typeHierarchyIndexer);
        }

        protected override void TidyUp()
        {
            Subject.Dispose();
        }
    }
}