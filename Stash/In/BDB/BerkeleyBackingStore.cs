#region License

// Copyright 2009 Andrew Hitchman
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

namespace Stash.In.BDB
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BerkeleyDB;
    using Engine;
    using global::Stash.Configuration;

    /// <summary>
    /// A backing store for Stash using BerkeleyDB.
    /// </summary>
    public class BerkeleyBackingStore : IBackingStore, IDisposable
    {
        public const string DatabaseFileExt = ".db";
        public const string IndexFilenamePrefix = "index-";
        public const string ReverseIndexFilenamePrefix = "ridx-";
        private const string concreteTypeFileName = "concreteTypes" + DatabaseFileExt;
        private const string graphFileName = "graphs" + DatabaseFileExt;

        private readonly IBerkeleyBackingStoreEnvironment backingStoreEnvironment;
        private bool isDisposed;

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        public BerkeleyBackingStore(IBerkeleyBackingStoreEnvironment backingStoreEnvironment)
        {
            this.backingStoreEnvironment = backingStoreEnvironment;
            IndexDatabases = new Dictionary<string, ManagedIndex>();

            ensureStashDirectory(backingStoreEnvironment);
            dbOpen();
        }

        public RegisteredIndexer<Type, Type> RegisteredTypeHierarchyIndex { get; private set; }

        public DatabaseEnvironment Environment
        {
            get { return backingStoreEnvironment.Environment; }
        }

        public HashDatabase GraphDatabase { get; private set; }
        public HashDatabase ConcreteTypeDatabase { get; private set; }
        public Dictionary<string, ManagedIndex> IndexDatabases { get; private set; }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;

            dbClose();
        }

        public void EnsureIndex(IRegisteredIndexer registeredIndexer)
        {
            var configForType = backingStoreEnvironment.IndexDatabaseConfigForTypes.ContainsKey(registeredIndexer.YieldType)
                                    ? backingStoreEnvironment.IndexDatabaseConfigForTypes[registeredIndexer.YieldType]
                                    : backingStoreEnvironment.IndexDatabaseConfigForTypes[typeof(object)];

            var indexDatabase =
                BTreeDatabase.Open(
                    IndexFilenamePrefix + registeredIndexer.IndexName + DatabaseFileExt,
                    configForType);

            var rIndexDatabase =
                HashDatabase.Open(
                    ReverseIndexFilenamePrefix + registeredIndexer.IndexName + DatabaseFileExt,
                    backingStoreEnvironment.ReverseIndexDatabaseConfig);

            IndexDatabases.Add(
                registeredIndexer.IndexName,
                new ManagedIndex(registeredIndexer.IndexName, registeredIndexer.YieldType, indexDatabase, rIndexDatabase, configForType));
        }

        public IStoredGraph Get(Guid internalId, IRegisteredGraph registeredGraph)
        {
            try
            {
                var key = new DatabaseEntry(internalId.AsByteArray());
                var storedConcreteType = ConcreteTypeDatabase.Get(key).Value.Data.AsString();
                if(storedConcreteType != registeredGraph.GraphType.FullName)
                    throw new AttemptToGetWithWrongRegisteredGraphException(internalId, storedConcreteType, registeredGraph);

                var entry = GraphDatabase.Get(key);
                return new StoredGraph(internalId, entry.Value.Data, registeredGraph);
            }
            catch(NotFoundException knfe)
            {
                throw new GraphForKeyNotFoundException(internalId, registeredGraph, knfe);
            }
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            var storageWork = new BerkeleyStorageWork(this);
            storageWorkActions(storageWork);
            storageWork.Commit();
        }

        ~BerkeleyBackingStore()
        {
            Dispose();
        }

        private void closeConcreteTypeDatabase()
        {
            if(ConcreteTypeDatabase != null) ConcreteTypeDatabase.Close();
        }

        private void closeEnvironment()
        {
            backingStoreEnvironment.Close();
        }

        private void closeGraphDatabase()
        {
            if(GraphDatabase != null) GraphDatabase.Close();
        }

        private void closeIndexDatabases()
        {
            foreach(var indexManager in IndexDatabases.Values)
            {
                indexManager.Close();
            }
        }

        private void dbClose()
        {
            closeIndexDatabases();
            closeConcreteTypeDatabase();
            closeGraphDatabase();

            closeEnvironment();
        }

        private void dbOpen()
        {
            openGraphDatabase();
            openConcreteTypeDatabase();

            RegisteredTypeHierarchyIndex = new RegisteredIndexer<Type, Type>(new StashTypeHierarchy());
            EnsureIndex(RegisteredTypeHierarchyIndex);
        }

        private static void ensureStashDirectory(IBerkeleyBackingStoreEnvironment backingStoreEnvironment)
        {
            Directory.CreateDirectory(Path.Combine(backingStoreEnvironment.DatabaseDirectory, backingStoreEnvironment.DatabaseEnvironmentConfig.CreationDir));
        }

        private void openConcreteTypeDatabase()
        {
            ConcreteTypeDatabase = HashDatabase.Open(concreteTypeFileName, backingStoreEnvironment.ValueDatabaseConfig);
        }

        private void openGraphDatabase()
        {
            GraphDatabase = HashDatabase.Open(graphFileName, backingStoreEnvironment.ValueDatabaseConfig);
        }
    }
}