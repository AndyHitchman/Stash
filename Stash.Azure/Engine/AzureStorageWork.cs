namespace Stash.Azure.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AzureQueries;
    using BackingStore;
    using Configuration;
    using Microsoft.WindowsAzure.StorageClient;
    using Queries;
    using Serializers;
    using Stash.Engine;

    public class AzureStorageWork : IStorageWork
    {
        private readonly AzureBackingStore backingStore;
        public readonly TableServiceContext ServiceContext;

        public AzureStorageWork(AzureBackingStore backingStore, CloudTableClient cloudTableClient)
        {
            this.backingStore = backingStore;
            ServiceContext = cloudTableClient.GetDataServiceContext();
        }

        public void Commit()
        {
            ServiceContext.SaveChangesWithRetries();
        }

        public int Count(IQuery query)
        {
            return executeQuery(query).Count();
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            //This Get is lazy, so the transaction that originally did the match is likely closed. 
            //We go back to the backing store to start a new transaction.
            return Matching(query).Distinct().Select(internalId => backingStore.Get(internalId));
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            return executeQuery(query);
        }

        public IStoredGraph Get(InternalId internalId)
        {
            var blobReference = backingStore.GraphContainer.GetBlobReference(internalId.ToString());
            var concreteType =
                (from ct in backingStore.ConcreteTypeQuery
                 where ct.PartitionKey == internalId.ToString()
                 select ct).First();
            var stream = new PreservedMemoryStream();
            blobReference.DownloadToStream(stream);
            return 
                new StoredGraph(
                    internalId, 
                    Type.GetType(concreteType.RowKey), 
                    backingStore.ConcurrencyPolicy.GetAccessConditionForModification(blobReference.Properties.ETag), 
                    stream);
        }

        public void InsertGraph(ITrackedGraph trackedGraph)
        {
            insertGraphData(trackedGraph);
            insertConcreteType(trackedGraph);
            insertTypeHierarchy(trackedGraph);
            insertIndexes(trackedGraph);
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            updateGraphData(trackedGraph);
            updateIndexes(trackedGraph);
        }

        public void DeleteGraph(InternalId internalId, IRegisteredGraph registeredGraph)
        {
            deleteIndexes(internalId, registeredGraph);
            deleteTypeHierarchy(internalId);
            deleteConcreteType(internalId, registeredGraph);
            deleteGraphData(internalId);
        }

        private void insertGraphData(ITrackedGraph trackedGraph)
        {
            var blob = backingStore.GraphContainer.GetBlockBlobReference(trackedGraph.StoredGraph.InternalId.ToString());
            blob.Properties.ContentType = trackedGraph.SerializedContentType;
            blob.UploadFromStream(
                trackedGraph.StoredGraph.SerialisedGraph,
                new BlobRequestOptions { AccessCondition = ((StoredGraph)trackedGraph.StoredGraph).AccessCondition });
        }

        private void updateGraphData(ITrackedGraph trackedGraph)
        {
            var blob = backingStore.GraphContainer.GetBlockBlobReference(trackedGraph.StoredGraph.InternalId.ToString());
            blob.Properties.ContentType = trackedGraph.SerializedContentType;
            blob.UploadFromStream(
                trackedGraph.StoredGraph.SerialisedGraph,
                new BlobRequestOptions { AccessCondition = ((StoredGraph)trackedGraph.StoredGraph).AccessCondition });
        }

        private void deleteGraphData(InternalId internalId)
        {
            var blob = backingStore.GraphContainer.GetBlockBlobReference(internalId.ToString());
            blob.Delete();
        }

        private void insertConcreteType(ITrackedGraph trackedGraph)
        {
            ServiceContext.AddObject(
                AzureBackingStore.ConcreteTypeTableName,
                new ConcreteTypeEntity
                    {
                        PartitionKey = trackedGraph.StoredGraph.InternalId.ToString(),
                        RowKey = trackedGraph.StoredGraph.GraphType.AssemblyQualifiedName
                    });
        }

        private void deleteConcreteType(InternalId internalId, IRegisteredGraph registeredGraph)
        {
            var cte = new ConcreteTypeEntity {PartitionKey = internalId.ToString(), RowKey = registeredGraph.GraphType.AssemblyQualifiedName};
            ServiceContext.AttachTo(AzureBackingStore.ConcreteTypeTableName, cte, "*");
            ServiceContext.DeleteObject(cte);
        }

        private void insertTypeHierarchy(ITrackedGraph trackedGraph)
        {
            var typeHierarchyIndex = backingStore.IndexDatabases[backingStore.RegisteredTypeHierarchyIndex.IndexName];

            foreach (var type in trackedGraph.TypeHierarchy)
            {
                typeHierarchyIndex.Insert(type, trackedGraph.StoredGraph.InternalId, ServiceContext);
            }
        }

        private void deleteTypeHierarchy(InternalId internalId)
        {
            var typeHierarchyIndex = backingStore.IndexDatabases[backingStore.RegisteredTypeHierarchyIndex.IndexName];
            deleteAllIndexEntriesForInternalId(typeHierarchyIndex, internalId);
        }

        private void insertIndexes(ITrackedGraph trackedGraph)
        {
            foreach (var projection in trackedGraph.ProjectedIndexes)
                insertIndex((ProjectedIndex)projection, trackedGraph.StoredGraph.InternalId);
        }

        private void updateIndexes(ITrackedGraph trackedGraph)
        {
            foreach (var index in trackedGraph.Indexes
                .Select(
                    index => new
                        {
                            managedIndex = backingStore.IndexDatabases[index.IndexName]
                        }))
            {
                deleteAllIndexEntriesForInternalId(index.managedIndex, trackedGraph.StoredGraph.InternalId);
            }

            insertIndexes(trackedGraph);
        }

        private void deleteIndexes(InternalId internalId, IRegisteredGraph registeredGraph)
        {
            foreach (var managedIndex in registeredGraph.IndexersOnGraph.Select(index => backingStore.IndexDatabases[index.IndexName]))
                deleteAllIndexEntriesForInternalId(managedIndex, internalId);
        }

        private void insertIndex(ProjectedIndex projection, InternalId internalId)
        {
            backingStore.IndexDatabases[projection.IndexName].Insert(projection.UntypedKey, internalId, ServiceContext);
        }

        private void deleteAllIndexEntriesForInternalId(ManagedIndex managedIndex, InternalId internalId)
        {
            managedIndex.Delete(internalId, ServiceContext);
        }

        private IEnumerable<InternalId> executeQuery(IQuery query)
        {
            var azureQuery = (IAzureQuery)query;
            return azureQuery.Execute(ServiceContext);
        }
    }
}