#region License
// Copyright 2009, 2010 Andrew Hitchman
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

namespace Stash.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Engine;
    using Microsoft.WindowsAzure;
    using Queries;
    using Microsoft.WindowsAzure.StorageClient;

    public class AzureBackingStore : IBackingStore, IDisposable
    {
        private bool isDisposed;
        private CloudTableClient cloudTableClient;
        private const string graphContainerName = "stash";
        public readonly string TypeHierarchyTableName = "stashtypehierarchies";
        public readonly string ConcreteTypeTableName = "stashconretetypes";

        public AzureBackingStore(CloudStorageAccount cloudStorageAccount)
        {
            GraphContainer =
                cloudStorageAccount
                    .CreateCloudBlobClient()
                    .GetContainerReference(graphContainerName);
            ensureGraphContainer();

            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            cloudTableClient.RetryPolicy = RetryPolicies.Retry(3, new TimeSpan(0, 0, 0, 0, 100));
            ensureTypeHierarchies();
            ensureConcreteTypes();
        }

        private void ensureGraphContainer() 
        {
            GraphContainer.CreateIfNotExist();
            var permissions = GraphContainer.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Off;
            GraphContainer.SetPermissions(permissions, new BlobRequestOptions {AccessCondition = AccessCondition.IfMatch(GraphContainer.Properties.ETag)});
        }

        private void ensureTypeHierarchies()
        {
            cloudTableClient.CreateTableIfNotExist(TypeHierarchyTableName);
        }

        private void ensureConcreteTypes()
        {
            cloudTableClient.CreateTableIfNotExist(ConcreteTypeTableName);
        }

        public CloudBlobContainer GraphContainer { get; private set; }

        public IQueryable<TableServiceEntity> TypeHierarchyQuery
        {
            get { return cloudTableClient.GetDataServiceContext().CreateQuery<TypeHierarchyEntity>(TypeHierarchyTableName); }
        }

        public IQueryable<TableServiceEntity> ConcreteTypeQuery
        {
            get { return cloudTableClient.GetDataServiceContext().CreateQuery<TypeHierarchyEntity>(ConcreteTypeTableName); }
        }

        public IQueryFactory QueryFactory
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
        }

        public int Count(IQuery query)
        {
            return InTransactionDo(work => work.Count(query));
        }

        public void EnsureIndex(IRegisteredIndexer registeredIndexer)
        {
            cloudTableClient.CreateTableIfNotExist()
        }

        public IProjectedIndex ProjectIndex<TKey>(string indexName, TKey key)
        {
            return new ProjectedIndex<TKey>(indexName, key);
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            return InTransactionDo(work => work.Get(query));
        }

        public IStoredGraph Get(InternalId internalId)
        {
            return InTransactionDo(work => work.Get(internalId));
        }

        public TReturn InTransactionDo<TReturn>(Func<IStorageWork, TReturn> storageWorkActions)
        {
            var storageWork = new AzureStorageWork(this, cloudTableClient);
            return storageWorkActions(storageWork);
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            var storageWork = new AzureStorageWork(this, cloudTableClient);
            storageWorkActions(storageWork);
        }

        public void Dispose()
        {
            if (isDisposed) return;
            isDisposed = true;

            Close();
        }
    }
}