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
    using System.Linq;
    using BerkeleyDB;
    using Engine;

    public class BerkeleyStorageWork : IStorageWork
    {
        public BerkeleyStorageWork(BerkeleyBackingStore backingStore)
        {
            BackingStore = backingStore;
            Transaction = backingStore.Environment.BeginTransaction();
        }

        public BerkeleyBackingStore BackingStore { get; private set; }
        public Transaction Transaction { get; private set; }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void DeleteGraph(Guid internalId)
        {
            BackingStore.GraphDatabase.Delete(new DatabaseEntry(internalId.AsByteArray()), Transaction);
            BackingStore.ConcreteTypeDatabase.Delete(new DatabaseEntry(internalId.AsByteArray()), Transaction);

            //How to delete indexes:
            //We aren't using secondary associated indexes in BDB because:
            // 1) (the .NET wrapper at least) only returns a single key for the data. We want to yield multiple keys.
            // 2) The key generator works on primitive byte arrays, which means we'd have to deserialise our graphs to calc indexes. Expensive.
            //So, we actually maintain a reverse index to give us a fast lookup of which index keys to delete. Disk space is cheap.
        }

        public IStoredGraph Get(Guid internalId)
        {
            throw new NotImplementedException();
        }

        public void InsertGraph(ITrackedGraph trackedGraph)
        {
            BackingStore.GraphDatabase.PutNoOverwrite(
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                new DatabaseEntry(trackedGraph.SerialisedGraph.ToArray()),
                Transaction);

            BackingStore.ConcreteTypeDatabase.PutNoOverwrite(
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                new DatabaseEntry(trackedGraph.ConcreteType.AsByteArray()),
                Transaction);

            insertTypeHierarchy(trackedGraph);
            insertIndexes(trackedGraph);
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            throw new NotImplementedException();
        }

        private void insertIndexes(ITrackedGraph trackedGraph)
        {
            foreach(var index in trackedGraph.Indexes)
            {
                var indexDatabase = BackingStore.IndexDatabases[index.IndexName];
                indexDatabase.IndexDatabase
                    .Put(
                        new DatabaseEntry(indexDatabase.IndexDatabaseConfig.AsByteArray(index.UntypedKey)),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        Transaction);
                indexDatabase.ReverseIndexDatabase
                    .Put(
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        new DatabaseEntry(indexDatabase.IndexDatabaseConfig.AsByteArray(index.UntypedKey)),
                        Transaction);
            }
        }

        private void insertTypeHierarchy(ITrackedGraph trackedGraph)
        {
            var typeHierarchyDatabase = BackingStore.IndexDatabases[BerkeleyBackingStore.TypeHierarchyIndexName];

            typeHierarchyDatabase.IndexDatabase
                .Put(
                    new DatabaseEntry(trackedGraph.ConcreteType.AsByteArray()),
                    new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                    Transaction);
            typeHierarchyDatabase.ReverseIndexDatabase
                .Put(
                    new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                    new DatabaseEntry(trackedGraph.ConcreteType.AsByteArray()),
                    Transaction);

            foreach(var type in trackedGraph.SuperTypes)
            {
                typeHierarchyDatabase.IndexDatabase
                    .Put(
                        new DatabaseEntry(type.FullName.AsByteArray()),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        Transaction);
                typeHierarchyDatabase.ReverseIndexDatabase
                    .Put(
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        new DatabaseEntry(type.FullName.AsByteArray()),
                        Transaction);
            }
        }
    }
}