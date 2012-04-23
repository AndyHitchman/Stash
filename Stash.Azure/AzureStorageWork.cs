namespace Stash.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AzureQueries;
    using BackingStore;
    using Configuration;
    using Engine;
    using Microsoft.WindowsAzure.StorageClient;
    using Queries;

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
            var graph = backingStore.GraphContainer.GetBlobReference(internalId.ToString());
            var concreteType =
                (from ct in backingStore.ConcreteTypeQuery
                 where ct.PartitionKey == internalId.ToString()
                 select ct).First();
            return new StoredGraph(internalId, graph.DownloadByteArray(), concreteType.RowKey);
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
            var blob = backingStore.GraphContainer.GetBlockBlobReference(trackedGraph.InternalId.ToString());
            blob.UploadByteArray(trackedGraph.SerialisedGraph.ToArray());
        }

        private void updateGraphData(ITrackedGraph trackedGraph)
        {
            var blob = backingStore.GraphContainer.GetBlockBlobReference(trackedGraph.InternalId.ToString());
            blob.UploadByteArray(trackedGraph.SerialisedGraph.ToArray());
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
                        PartitionKey = trackedGraph.InternalId.ToString(),
                        RowKey = trackedGraph.GraphType.AssemblyQualifiedName
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
                typeHierarchyIndex.Insert(type, trackedGraph.InternalId, ServiceContext);
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
                insertIndex((ProjectedIndex)projection, trackedGraph.InternalId);
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
                deleteAllIndexEntriesForInternalId(index.managedIndex, trackedGraph.InternalId);
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