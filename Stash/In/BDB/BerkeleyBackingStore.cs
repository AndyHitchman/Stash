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
    using Configuration;
    using Engine;

    /// <summary>
    /// A backing store for Stash using BerkeleyDB.
    /// </summary>
    public class BerkeleyBackingStore : IBackingStore, IDisposable
    {
        private const string DatabaseFileExt = ".db";
        private const string ConcreteTypeFileName = "concreteTypes" + DatabaseFileExt;
        private const string GraphFileName = "graphs" + DatabaseFileExt;
        private const string IndexFilenamePrefix = "index-";
        private const string ReverseIndexFilenamePrefix = "ridx-";
        public const string TypeHierarchyIndexName = "stashTypeHierarchy";

        private readonly IBerkeleyBackingStoreEnvironment backingStoreEnvironment;
        private bool isDisposed;

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        public BerkeleyBackingStore(IBerkeleyBackingStoreEnvironment backingStoreEnvironment)
        {
            this.backingStoreEnvironment = backingStoreEnvironment;
            Directory.CreateDirectory(Path.Combine(backingStoreEnvironment.DatabaseDirectory, backingStoreEnvironment.DatabaseEnvironmentConfig.CreationDir));
            IndexDatabases = new Dictionary<string, IndexManager>();

            dbOpen();
        }

        public DatabaseEnvironment Environment { get { return backingStoreEnvironment.Environment; } }

        public HashDatabase GraphDatabase { get; private set; }
        public HashDatabase ConcreteTypeDatabase { get; private set; }
        public Dictionary<string, IndexManager> IndexDatabases { get; private set; }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;

            dbClose();
        }

        public void EnsureIndex(string indexName, Type yieldsType)
        {
            var indexDatabaseConfigForType = backingStoreEnvironment.IndexDatabaseConfigForTypes.ContainsKey(yieldsType)
                                                 ? backingStoreEnvironment.IndexDatabaseConfigForTypes[yieldsType]
                                                 : backingStoreEnvironment.IndexDatabaseConfigForTypes[typeof(object)];

            var indexDatabase = BTreeDatabase.Open(IndexFilenamePrefix + indexName + DatabaseFileExt, indexDatabaseConfigForType);
            var rIndexDatabase = HashDatabase.Open(ReverseIndexFilenamePrefix + indexName + DatabaseFileExt, backingStoreEnvironment.ReverseIndexDatabaseConfig);
            IndexDatabases.Add(indexName, new IndexManager(indexName, yieldsType, indexDatabase, rIndexDatabase, indexDatabaseConfigForType));
        }

        public IStoredGraph Get(Guid internalId)
        {
            throw new NotImplementedException();
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

            EnsureIndex(TypeHierarchyIndexName, typeof(Type));
        }

        private void openConcreteTypeDatabase()
        {
            ConcreteTypeDatabase = HashDatabase.Open(ConcreteTypeFileName, backingStoreEnvironment.ValueDatabaseConfig);
        }

        private void openGraphDatabase()
        {
            GraphDatabase = HashDatabase.Open(GraphFileName, backingStoreEnvironment.ValueDatabaseConfig);
        }
    }
}