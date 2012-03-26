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
        private TableServiceContext serviceContext;

        public AzureStorageWork(AzureBackingStore backingStore, CloudTableClient cloudTableClient)
        {
            this.backingStore = backingStore;
            serviceContext = cloudTableClient.GetDataServiceContext();
        }

        public int Count(IQuery query)
        {
            return executeQuery(query).Count();
        }

        public void DeleteGraph(InternalId internalId, IRegisteredGraph registeredGraph)
        {
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            //This Get is lazy, so the transaction that originally did the match is likely closed. 
            //We go back to the backing store to start a new transaction.
            return Matching(query).Select(internalId => backingStore.Get(internalId));
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            return executeQuery(query);
        }

        public IStoredGraph Get(InternalId internalId)
        {
            var graph = backingStore.GraphContainer.GetBlobReference(internalId.Value.ToString());
            var concreteType =
                (from ct in backingStore.TypeHierarchyQuery
                 where ct.PartitionKey == internalId.Value.ToString()
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

        private void insertGraphData(ITrackedGraph trackedGraph)
        {
            var blob = backingStore.GraphContainer.GetBlockBlobReference(trackedGraph.InternalId.Value.ToString());
            blob.UploadByteArray(trackedGraph.SerialisedGraph.ToArray());
        }

        private void insertConcreteType(ITrackedGraph trackedGraph)
        {
            serviceContext.AddObject(
                backingStore.ConcreteTypeTableName,
                new ConcreteTypeEntity
                    {
                        PartitionKey = trackedGraph.InternalId.Value.ToString(),
                        RowKey = trackedGraph.GraphType.AssemblyQualifiedName
                    });
        }

        private void insertTypeHierarchy(ITrackedGraph trackedGraph)
        {
            foreach (var type in trackedGraph.TypeHierarchy)
            {
                serviceContext.AddObject(
                    backingStore.TypeHierarchyTableName,
                    new TypeHierarchyEntity
                        {
                            PartitionKey = type,
                            RowKey = trackedGraph.InternalId.Value.ToString()
                        });
            }
        }

        private void insertIndexes(ITrackedGraph trackedGraph)
        {
            foreach (var projection in trackedGraph.ProjectedIndexes)
                insertIndex((ProjectedIndex)projection, trackedGraph.InternalId);
        }

        private void insertIndex(ProjectedIndex projection, InternalId internalId)
        {
            serviceContext.AddObject();
            managedIndex.Insert(projection.UntypedKey, internalId, Transaction);
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<InternalId> executeQuery(IQuery query)
        {
            var azureQuery = (IAzureQuery)query;
            return azureQuery.Execute();
        }
    }
}