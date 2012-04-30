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
    using System.Net;
    using BackingStore;
    using Configuration;
    using Engine;
    using JsonSerializer;
    using Microsoft.WindowsAzure;
    using Queries;
    using Microsoft.WindowsAzure.StorageClient;
    using Serializers;

    public class AzureBackingStore : IBackingStore, IDisposable
    {
        private bool isDisposed;
        private const string graphContainerName = "stash";
        public const string ConcreteTypeTableName = "stashconretetypes";

        public AzureBackingStore(CloudStorageAccount cloudStorageAccount)
        {
            configureServiceEndpoint(cloudStorageAccount);

            GraphContainer =
                cloudStorageAccount
                    .CreateCloudBlobClient()
                    .GetContainerReference(graphContainerName);
            ensureGraphContainer();

            CloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTableClient.RetryPolicy = RetryPolicies.Retry(3, new TimeSpan(0, 0, 0, 0, 100));

            ensureConcreteTypes();
            IndexDatabases = new Dictionary<string, ManagedIndex>();
            RegisteredTypeHierarchyIndex = new RegisteredIndexer<Type, string>(new StashTypeHierarchy(), null);

            QueryFactory = new AzureQueryFactory(this);
        }

        private void configureServiceEndpoint(CloudStorageAccount cloudStorageAccount)
        {
            var servicePoint = ServicePointManager.FindServicePoint(cloudStorageAccount.TableEndpoint);
            servicePoint.ConnectionLimit = 16 * Environment.ProcessorCount;
            servicePoint.Expect100Continue = false;
        }

        private void ensureGraphContainer() 
        {
            GraphContainer.CreateIfNotExist();
            var permissions = GraphContainer.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Off;
            GraphContainer.SetPermissions(permissions, new BlobRequestOptions {AccessCondition = AccessCondition.IfMatch(GraphContainer.Properties.ETag)});
        }

        private void ensureConcreteTypes()
        {
            CloudTableClient.CreateTableIfNotExist(ConcreteTypeTableName);
        }

        public CloudTableClient CloudTableClient { get; private set; }
        public CloudBlobContainer GraphContainer { get; private set; }
        public Dictionary<string, ManagedIndex> IndexDatabases { get; private set; }

        public TableServiceContext TableServiceContext
        {
            get { return CloudTableClient.GetDataServiceContext(); }
        }

        public IQueryable<TableServiceEntity> ConcreteTypeQuery
        {
            get { return CloudTableClient.GetDataServiceContext().CreateQuery<TypeHierarchyEntity>(ConcreteTypeTableName); }
        }

        public IQueryFactory QueryFactory { get; private set; }

        public RegisteredIndexer<Type, string> RegisteredTypeHierarchyIndex { get; private set; }

        public void Close()
        {
        }

        public int Count(IQuery query)
        {
            return InTransactionDo(work => work.Count(query));
        }

        public void EnsureIndex(IRegisteredIndexer registeredIndexer)
        {
            var managedIndex = new ManagedIndex(this, registeredIndexer);

            IndexDatabases.Add(
                registeredIndexer.IndexName,
                managedIndex);

            managedIndex.EnsureIndex(CloudTableClient);
        }

        public IProjectedIndex ProjectIndex<TKey>(string indexName, TKey key)
        {
            return new ProjectedIndex<TKey>(indexName, key);
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            return InTransactionDo(work => work.Matching(query));
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
            var storageWork = new AzureStorageWork(this, CloudTableClient);
            try
            {
                return storageWorkActions(storageWork);
            }
            finally
            {
                storageWork.Commit();
            }
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            var storageWork = new AzureStorageWork(this, CloudTableClient);
            try
            {
                storageWorkActions(storageWork);
            }
            finally
            {
                storageWork.Commit();
            }
        }

        public bool IsTypeASupportedInIndexes(Type proposedIndexType)
        {
            return Convert.For.ContainsKey(proposedIndexType);
        }

        public ISerializer<TGraph> GetDefaultSerialiser<TGraph>(IRegisteredGraph<TGraph> registeredGraph)
        {
            return new JsonSerializer<TGraph>(registeredGraph);
        }

        public void Dispose()
        {
            if (isDisposed) return;
            isDisposed = true;

            Close();
        }
    }
}