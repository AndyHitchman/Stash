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

namespace Stash.BackingStore.BDB
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using BerkeleyConfigs;
    using BerkeleyDB;
    using Configuration;
    using Engine;

    public class ManagedIndex
    {
        private readonly BerkeleyBackingStore backingStore;
        private readonly IBerkeleyBackingStoreEnvironment environment;
        private readonly IRegisteredIndexer registeredIndexer;
        public const string IndexFilenamePrefix = "index-";
        public const string ReverseIndexFilenamePrefix = "ridx-";
        private const int recompileTimeoutMs = 1000 * 10;

        private IndexDatabaseConfig config;
        private EventWaitHandle indexCompiled;

        public ManagedIndex(BerkeleyBackingStore backingStore, IBerkeleyBackingStoreEnvironment environment, IRegisteredIndexer registeredIndexer)
        {
            this.backingStore = backingStore;
            this.environment = environment;
            this.registeredIndexer = registeredIndexer;

            indexCompiled = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public void EnsureIndex()
        {
            Name = registeredIndexer.IndexName;
            config = environment.IndexDatabaseConfigForTypes.ContainsKey(registeredIndexer.YieldType)
                         ? environment.IndexDatabaseConfigForTypes[registeredIndexer.YieldType]
                         : environment.IndexDatabaseConfigForTypes[typeof(object)];

            //Before we open (with create if needed).
            var recompileRequired = RecompileIsRequired();

            index =
                BTreeDatabase.Open(
                    getIndexFilename(),
                    config);

            reverseIndex =
                HashDatabase.Open(
                    getReverseIndexFilename(),
                    environment.ReverseIndexDatabaseConfig);

            if (recompileRequired)
                BuildIndexFromGraphs();
            else
                indexCompiled.Set();
        }

        public bool RecompileIsRequired()
        {
            return !(indexIsTypeHierarchy() || indexExists());
        }

        private bool indexIsTypeHierarchy()
        {
            return registeredIndexer.IndexType == typeof(StashTypeHierarchy);
        }

        private bool indexExists() {
            var indexPath = Path.Combine(
                Path.Combine(environment.DatabaseDirectory, environment.DatabaseEnvironmentConfig.CreationDir), getIndexFilename());
            return File.Exists(indexPath);
        }

        private string getReverseIndexFilename()
        {
            return ReverseIndexFilenamePrefix + registeredIndexer.IndexName + BerkeleyBackingStore.DatabaseFileExt;
        }

        private string getIndexFilename()
        {
            return IndexFilenamePrefix + registeredIndexer.IndexName + BerkeleyBackingStore.DatabaseFileExt;
        }

        public void BuildIndexFromGraphs()
        {
            //Let the background thread claim the wait handle.
//            ThreadPool.QueueUserWorkItem(
//                state =>
//                    {
                        var registeredGraphs = registeredIndexer.GraphsIndexed;
                        var session = new InternalSession(registeredIndexer.Registry, backingStore);

                        //We use the session to manage to loading and deserialisation of graphs the contribute to the index...
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
                                    (graph, projection) => new {Projection = projection, graph.InternalId});

                        backingStore.InTransactionDo(
                            work =>
                                {
                                    foreach(var projection in graphProjections)
                                    {
                                        Insert(projection.Projection.UntypedKey, projection.InternalId, ((BerkeleyStorageWork)work).Transaction);
                                    }
                                });

                        session.Abandon();

                        indexCompiled.Set();
//                    });
        }

        public void Insert(object untypedKey, InternalId internalId, Transaction transaction)
        {
            index
                .Put(
                    new DatabaseEntry(KeyAsByteArray(untypedKey)),
                    new DatabaseEntry(internalId.AsByteArray()),
                    transaction);
            reverseIndex
                .Put(
                    new DatabaseEntry(internalId.AsByteArray()),
                    new DatabaseEntry(KeyAsByteArray(untypedKey)),
                    transaction);
        }


        public void Delete(DatabaseEntry graphKey, Transaction transaction)
        {
            //Get all reverse index values for the internal id.
            var allKeysInIndex =
                reverseIndex
                    .GetMultiple(graphKey, (int)reverseIndex.Pagesize, transaction);

            //In the forward index, get all keys for each reverse index value and delete the record if the value matches 
            //the internal id of the graph we are deleting.
            foreach (var cursor in
                from indexKey in allKeysInIndex.Value
                let c = index.Cursor(new CursorConfig(), transaction)
                where c.Move(indexKey, true)
                select c)
            {
                do
                {
                    if (cursor.Current.Value.Data.SequenceEqual(graphKey.Data)) cursor.Delete();
                }
                while (cursor.MoveNextDuplicate());

                cursor.Close();
            }

            reverseIndex.Delete(graphKey, transaction);
        }


        public string Name { get; set; }

        public BTreeDatabase Index
        {
            get
            {
                //Give the background thread time to finish recompiling. 
                if(indexCompiled.WaitOne(recompileTimeoutMs))
                    return index;

                throw new IndexNotReadyException(Name);
            }
        }

        private BTreeDatabase index;
 
        public HashDatabase ReverseIndex
        {
            get
            {
                if (indexCompiled.WaitOne(recompileTimeoutMs))
                    return reverseIndex;

                throw new IndexNotReadyException(Name);
            }
        }

        private HashDatabase reverseIndex;


        public Type YieldsType { get; private set; }

        public IComparer Comparer
        {
            get { return config.GetComparer(); }
        }

        public object ByteArrayAsKey(byte[] bytes)
        {
            return config.ByteArrayAsKey(bytes);
        }

        public void Close()
        {
            if(index != null)
                index.Close();
            
            if(reverseIndex != null)
                reverseIndex.Close();
        }

        public byte[] KeyAsByteArray(object key)
        {
            return config.KeyAsByteArray(key);
        }
    }
}