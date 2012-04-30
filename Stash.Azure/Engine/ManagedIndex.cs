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

namespace Stash.Azure.Engine
{
    using System;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BackingStore;
    using Configuration;
    using Microsoft.WindowsAzure.StorageClient;
    using Stash.Engine;

    public class ManagedIndex
    {
        private readonly AzureBackingStore backingStore;
        private readonly IRegisteredIndexer registeredIndexer;
        private const string indexFilenamePrefix = "idx";
        private const string reverseIndexFilenamePrefix = "rdx";
        private const int recompileTimeoutMs = 1000 * 60;
        private readonly EventWaitHandle indexCompiled;
        private readonly string tableName;

        public ManagedIndex(AzureBackingStore backingStore, IRegisteredIndexer registeredIndexer)
        {
            this.backingStore = backingStore;
            this.registeredIndexer = registeredIndexer;

            indexCompiled = new EventWaitHandle(false, EventResetMode.ManualReset);
            tableName = safeTableName(registeredIndexer.IndexName);
        }

        private static string safeTableName(string indexName)
        {
            var safeChars = new String(indexName.Where((c, i) => i > 0 ? char.IsLetterOrDigit(c) : char.IsLetter(c)).ToArray());
            if (safeChars.Length < 1)
                throw new ArgumentException(
                    string.Format("index name {0} derived an empty safe table name {1} which is not 3 characters or more is length", indexName, safeChars), 
                    "indexName");

            var maxTableNameLength = 63 - (indexFilenamePrefix.Length > reverseIndexFilenamePrefix.Length ? indexFilenamePrefix.Length : reverseIndexFilenamePrefix.Length);
            if (safeChars.Length > maxTableNameLength)
                safeChars = safeChars.Substring(safeChars.Length - maxTableNameLength);

            return safeChars;
        }

        public void EnsureIndex(CloudTableClient cloudTableClient)
        {
            Name = registeredIndexer.IndexName;

            //Before we open (with create if needed).
            var recompileRequired = RecompileIsRequired(cloudTableClient);

//            if (recompileRequired)
//                BuildIndexFromGraphs(cloudTableClient);
//            else
                indexCompiled.Set();
        }

        public bool RecompileIsRequired(CloudTableClient cloudTableClient)
        {
            return !(indexIsTypeHierarchy() | ensureIndexExists(cloudTableClient));
        }

        private bool indexIsTypeHierarchy()
        {
            return registeredIndexer.IndexType == typeof(StashTypeHierarchy);
        }

        private bool ensureIndexExists(CloudTableClient cloudTableClient) {
            return !cloudTableClient.CreateTableIfNotExist(ForwardIndexName) | !cloudTableClient.CreateTableIfNotExist(ReverseIndexName);
        }

        public string ReverseIndexName
        {
            get { return reverseIndexFilenamePrefix + tableName; }
        }

        public string ForwardIndexName
        {
            get { return indexFilenamePrefix + tableName; }
        }

        public void BuildIndexFromGraphs(CloudTableClient cloudTableClient)
        {
            //Let the background thread claim the wait handle.
            ThreadPool.QueueUserWorkItem(
                state =>
                    {
                        var registeredGraphs = registeredIndexer.GraphsIndexed;
                        var session = new InternalSession(registeredIndexer.Registry, backingStore);

                        //We use the session to manage to loading and deserialisation of graphs that contribute to the index...
                        session
                            .GetEntireStash()
                            .Matching(
                                _ =>
                                _.Where<StashTypeHierarchy>().AnyOf(registeredGraphs.Select(rg => StashTypeHierarchy.GetConcreteTypeValue(rg.GraphType))))
                            .Materialize();

                        //...but we then use the enrolled persistence events directly to yield the keys for this new index.
                        var graphProjections =
                            session
                                .EnrolledPersistenceEvents
                                .SelectMany(
                                    trackedGraph => registeredIndexer.GetUntypedProjections(trackedGraph.UntypedGraph),
                                    (graph, projection) => new {Projection = (ProjectedIndex)projection, graph.InternalId});

                        backingStore.InTransactionDo(
                            work =>
                                {
                                    foreach(var projection in graphProjections)
                                    {
                                        try
                                        {
                                            Insert(projection.Projection.UntypedKey, projection.InternalId, ((AzureStorageWork)work).ServiceContext);
                                        }
                                        catch(InvalidOperationException ioEx)
                                        {
                                            var storageEx = ioEx.TranslateDataServiceClientException();
                                            if (storageEx.ExtendedErrorInformation.ErrorCode != TableErrorCodeStrings.EntityAlreadyExists)
                                                throw storageEx;
                                            //This is acceptable, as it is possible for another session to insert the key
                                            //before we match it. We carry on.
                                        }
                                    }
                                });

                        session.Abandon();

                        indexCompiled.Set();
                    });
        }

        public void Insert(object key, InternalId internalId, TableServiceContext serviceContext)
        {
            serviceContext.AddObject(ForwardIndexName, new IndexEntity {PartitionKey = KeyAsString(key), RowKey = internalId.ToString(), OriginalValue = key.ToString()});
            serviceContext.AddObject(ReverseIndexName, new IndexEntity { PartitionKey = internalId.ToString(), RowKey = KeyAsString(key), OriginalValue = key.ToString() });
        }


        public void Delete(InternalId internalId, TableServiceContext serviceContext)
        {
            var allKeysInIndex =
                (from ri in ReverseIndex(serviceContext)
                 where ri.PartitionKey == internalId.ToString()
                 select ri);

            //In the forward index, get the corresponding forward index for each reverse index value and delete them both
            Parallel.ForEach(
                allKeysInIndex,
                reverseEntity =>
                    {
//                        var forwardEntity = new IndexEntity {PartitionKey = reverseEntity.RowKey, RowKey = reverseEntity.PartitionKey};
//                        serviceContext.AttachTo(ForwardIndexName, forwardEntity, "*");
                        var forwardEntity =
                            Queryable.Single<IndexEntity>((from fi in ForwardIndex(serviceContext)
                                                             where fi.PartitionKey == reverseEntity.RowKey
                                                             where fi.RowKey == reverseEntity.PartitionKey
                                                             select fi));
                        serviceContext.DeleteObject(forwardEntity);
                        serviceContext.DeleteObject(reverseEntity);
                    });
        }


        public string Name { get; set; }

        public DataServiceQuery<IndexEntity> ForwardIndex(TableServiceContext serviceContext)
        {
            //Give the background thread time to finish recompiling. 
            if(indexCompiled.WaitOne(recompileTimeoutMs))
                return serviceContext.CreateQuery<IndexEntity>(ForwardIndexName);

            throw new IndexNotReadyException(Name);
        }

        public DataServiceQuery<IndexEntity> ReverseIndex(TableServiceContext serviceContext)
        {
            if(indexCompiled.WaitOne(recompileTimeoutMs))
                return serviceContext.CreateQuery<IndexEntity>(ReverseIndexName);

            throw new IndexNotReadyException(Name);
        }

        public InternalId ConvertToInternalId(string stringRepresentation)
        {
            return new InternalId(Guid.Parse(stringRepresentation));
        }

        public object ConvertToKey(string stringRepresentation)
        {
            return Convert.For[registeredIndexer.YieldType].AsObject(stringRepresentation);
        }

        public string KeyAsString(object key)
        {
            return Convert.For[registeredIndexer.YieldType].IntoString(key);
        }
    }
}