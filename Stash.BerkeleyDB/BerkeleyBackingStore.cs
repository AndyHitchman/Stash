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

namespace Stash.BerkeleyDB
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using global::BerkeleyDB;
    using Serializers;
    using Serializers.Binary;
    using Stash.Configuration;
    using Stash.Engine;
    using Stash.Queries;

    /// <summary>
    /// A backing store for Stash using BerkeleyDB.
    /// </summary>
    public class BerkeleyBackingStore : IBackingStore, IDisposable
    {
        public const string DatabaseFileExt = ".db";
        private const string concreteTypeFileName = "concreteTypes" + DatabaseFileExt;
        private const string graphFileName = "graphs" + DatabaseFileExt;

        private readonly IBerkeleyBackingStoreEnvironment backingStoreEnvironment;
        private readonly BerkeleyQueryFactory queryFactory;
        private bool isClosed;
        private bool isDisposed;

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        public BerkeleyBackingStore(IBerkeleyBackingStoreEnvironment backingStoreEnvironment)
        {
            queryFactory = new BerkeleyQueryFactory(this);
            this.backingStoreEnvironment = backingStoreEnvironment;
            IndexDatabases = new Dictionary<string, ManagedIndex>();

            dbOpen();
        }

        public RegisteredIndexer<Type, string> RegisteredTypeHierarchyIndex { get; private set; }

        public IBerkeleyBackingStoreEnvironment Environment
        {
            get { return backingStoreEnvironment; }
        }

        public HashDatabase GraphDatabase { get; private set; }
        public HashDatabase ConcreteTypeDatabase { get; private set; }
        public Dictionary<string, ManagedIndex> IndexDatabases { get; private set; }

        public IQueryFactory QueryFactory
        {
            get { return queryFactory; }
        }

        public void Close()
        {
            if(isClosed) return;
            isClosed = true;

            closeIndexDatabases();
            closeConcreteTypeDatabase();
            closeGraphDatabase();

            closeEnvironment();
        }

        public int Count(IQuery query)
        {
            return InTransactionDo(work => work.Count(query));
        }

        public void Dispose()
        {
            if(isDisposed) return;
            isDisposed = true;

            Close();
        }

        public void EnsureIndex(IRegisteredIndexer registeredIndexer)
        {
            var managedIndex = new ManagedIndex(this, Environment, registeredIndexer);

            IndexDatabases.Add(
                registeredIndexer.IndexName,
                managedIndex);

            managedIndex.EnsureIndex();
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

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            var storageWork = new BerkeleyStorageWork(this);
            try
            {
                storageWorkActions(storageWork);
                storageWork.Commit();
            }
            catch
            {
                try
                {
                    storageWork.Abort();
                }
                catch {}

                throw;
            }
        }

        List<Type> supportedTypes = new List<Type>
            {
                typeof(string),
                typeof(Type), 
                typeof(TimeSpan),
                typeof(DateTime),
                typeof(bool),
                typeof(char),  
                typeof(decimal),
                typeof(int),
                typeof(long), 
                typeof(short),
                typeof(uint),
                typeof(ulong),
                typeof(ushort),
            };
        
        public bool IsTypeASupportedInIndexes(Type proposedIndexType)
        {
            return supportedTypes.Contains(proposedIndexType);
        }

        public ISerializer<TGraph> GetDefaultSerialiser<TGraph>(IRegisteredGraph<TGraph> registeredGraph)
        {
            return new AggregateBinarySerializer<TGraph>(registeredGraph);
        }

        public TReturn InTransactionDo<TReturn>(Func<IStorageWork, TReturn> storageWorkFunction)
        {
            var storageWork = new BerkeleyStorageWork(this);
            try
            {
                var result = storageWorkFunction(storageWork);
                storageWork.Commit();
                return result;
            }
            catch
            {
                storageWork.Abort();
                throw;
            }
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

        private void dbOpen()
        {
            openGraphDatabase();
            openConcreteTypeDatabase();

            RegisteredTypeHierarchyIndex = new RegisteredIndexer<Type, string>(new StashTypeHierarchy(), null);
            //Type hierarchy index is registered by the kernel, as it is used at the application level.
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