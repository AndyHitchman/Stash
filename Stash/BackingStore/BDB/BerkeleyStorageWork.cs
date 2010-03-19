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

namespace Stash.BackingStore.BDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using BerkeleyQueries;
    using Configuration;
    using Queries;
    using Engine;

    public class BerkeleyStorageWork : IStorageWork
    {
        public BerkeleyStorageWork(BerkeleyBackingStore backingStore)
        {
            BackingStore = backingStore;
            Transaction = backingStore.Environment.BeginTransaction();
        }

        public BerkeleyBackingStore BackingStore { get; set; }
        public Transaction Transaction { get; set; }

        public void Abort()
        {
            Transaction.Abort();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public int Count(IQuery query)
        {
            return executeQuery(query).Count();
        }

        public void DeleteGraph(Guid internalId, IRegisteredGraph registeredGraph)
        {
            var graphKey = new DatabaseEntry(internalId.AsByteArray());

            deleteIndexes(graphKey, registeredGraph);
            deleteTypeHierarchy(graphKey);
            deleteConcreteType(graphKey);
            deleteGraphData(graphKey);
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            return executeQuery(query)
                .Select(internalId => Get(internalId))
                .Materialize();
        }

        public IStoredGraph Get(Guid internalId)
        {
            try
            {
                var key = new DatabaseEntry(internalId.AsByteArray());
                var storedConcreteType = BackingStore.ConcreteTypeDatabase.Get(key, Transaction).Value.Data.AsString();

                var entry = BackingStore.GraphDatabase.Get(key, Transaction);
                return new StoredGraph(internalId, entry.Value.Data, storedConcreteType);
            }
            catch(NotFoundException knfe)
            {
                throw new GraphForKeyNotFoundException(internalId, knfe);
            }
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

        private void deleteAllIndexEntriesForGraphKey(ManagedIndex managedIndex, DatabaseEntry graphKey)
        {
            //Get all reverse index values for the internal id.
            var allKeysInIndex =
                managedIndex.ReverseIndex
                    .GetMultiple(graphKey, (int)managedIndex.ReverseIndex.Pagesize, Transaction);

            //In the forward index, get all keys for each reverse index value and delete the record if the value matches 
            //the internal id of the graph we are deleting.
            foreach(var cursor in
                from indexKey in allKeysInIndex.Value
                let c = managedIndex.Index.Cursor(new CursorConfig(), Transaction)
                where c.Move(indexKey, true)
                select c)
            {
                do
                {
                    if(cursor.Current.Value.Data.SequenceEqual(graphKey.Data)) cursor.Delete();
                }
                while(cursor.MoveNextDuplicate());

                cursor.Close();
            }

            managedIndex.ReverseIndex.Delete(graphKey, Transaction);
        }

        private void deleteConcreteType(DatabaseEntry graphKey)
        {
            BackingStore.ConcreteTypeDatabase.Delete(graphKey, Transaction);
        }

        private void deleteGraphData(DatabaseEntry graphKey)
        {
            BackingStore.GraphDatabase.Delete(graphKey, Transaction);
        }

        /// <summary>
        /// How to delete indexes:
        /// We aren't using secondary associated indexes in BDB because:
        ///  1) (the .NET wrapper at least) only returns a single key for the data. We want to yield multiple keys.
        ///  2) The key generator works on primitive byte arrays, which means we'd have to deserialise our graphs to calc indexes. Expensive.
        /// So, we actually maintain a reverse index to give us a fast lookup of which index keys to delete. Disk space is cheap.
        /// Internally BDB calculates the key by calling the delegate and then opens a cursor to delete the matching keys for the primary key.
        /// The reverse index should be faster, but obviously consumed double the space for each index.
        /// </summary>
        /// <param name="graphKey"></param>
        /// <param name="registeredGraph"></param>
        private void deleteIndexes(DatabaseEntry graphKey, IRegisteredGraph registeredGraph)
        {
            foreach(var managedIndex in registeredGraph.IndexersOnGraph.Select(index => BackingStore.IndexDatabases[index.IndexName]))
            {
                deleteAllIndexEntriesForGraphKey(managedIndex, graphKey);
            }
        }

        private void deleteTypeHierarchy(DatabaseEntry graphKey)
        {
            var typeHierarchyDatabase = BackingStore.IndexDatabases[BackingStore.RegisteredTypeHierarchyIndex.IndexName];
            deleteAllIndexEntriesForGraphKey(typeHierarchyDatabase, graphKey);
        }

        private IEnumerable<Guid> executeQuery(IQuery query)
        {
            var berkeleyQuery = (IBerkeleyQuery)query;
            return berkeleyQuery.Execute(Transaction);
        }

        private void insertConcreteType(ITrackedGraph trackedGraph)
        {
            BackingStore.ConcreteTypeDatabase.PutNoOverwrite(
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                new DatabaseEntry(trackedGraph.GraphType.AsByteArray()),
                Transaction);
        }

        private void insertGraphData(ITrackedGraph trackedGraph)
        {
            BackingStore.GraphDatabase.PutNoOverwrite(
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                new DatabaseEntry(trackedGraph.SerialisedGraph.ToArray()),
                Transaction);
        }

        private void insertIndexes(ITrackedGraph trackedGraph)
        {
            foreach(var each in 
                from projection in trackedGraph.ProjectedIndexes
                let managedIndex = BackingStore.IndexDatabases[projection.IndexName]
                select new {projection, managedIndex})
            {
                each.managedIndex.Index
                    .Put(
                        new DatabaseEntry(each.managedIndex.KeyAsByteArray(each.projection.UntypedKey)),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        Transaction);
                each.managedIndex.ReverseIndex
                    .Put(
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        new DatabaseEntry(each.managedIndex.KeyAsByteArray(each.projection.UntypedKey)),
                        Transaction);
            }
        }

        private void insertTypeHierarchy(ITrackedGraph trackedGraph)
        {
            var typeHierarchyDatabase = BackingStore.IndexDatabases[BackingStore.RegisteredTypeHierarchyIndex.IndexName];

            foreach(var type in trackedGraph.TypeHierarchy)
            {
                typeHierarchyDatabase.Index
                    .Put(
                        new DatabaseEntry(type.AsByteArray()),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        Transaction);
                typeHierarchyDatabase.ReverseIndex
                    .Put(
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        new DatabaseEntry(type.AsByteArray()),
                        Transaction);
            }
        }

        private void updateGraphData(ITrackedGraph trackedGraph)
        {
            BackingStore.GraphDatabase.Put(
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                new DatabaseEntry(trackedGraph.SerialisedGraph.ToArray()),
                Transaction);
        }

        private void updateIndexes(ITrackedGraph trackedGraph)
        {
            var graphKey = new DatabaseEntry(trackedGraph.InternalId.AsByteArray());

            foreach(var index in trackedGraph.Indexes
                .Select(
                    index => new
                        {
                            keys = trackedGraph.ProjectedIndexes.Where(_ => _.IndexName == index.IndexName).Select(_ => _.UntypedKey),
                            managedIndex = BackingStore.IndexDatabases[index.IndexName]
                        }))
            {
                deleteAllIndexEntriesForGraphKey(index.managedIndex, graphKey);
            }

            insertIndexes(trackedGraph);
        }
    }
}