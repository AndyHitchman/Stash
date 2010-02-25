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
    using System.Linq;
    using BerkeleyDB;
    using Engine;
    using global::Stash.Configuration;

    public class BerkeleyStorageWork : IStorageWork
    {
        public BerkeleyStorageWork(BerkeleyBackingStore backingStore)
        {
            this.backingStore = backingStore;
            transaction = backingStore.Environment.BeginTransaction();
        }

        private BerkeleyBackingStore backingStore { get; set; }
        private Transaction transaction { get; set; }

        public void Commit()
        {
            transaction.Commit();
        }

        public void DeleteGraph(Guid internalId, IRegisteredGraph registeredGraph)
        {
            var graphKey = new DatabaseEntry(internalId.AsByteArray());

            deleteIndexes(graphKey, registeredGraph);
            deleteTypeHierarchy(graphKey);
            deleteConcreteType(graphKey);
            deleteGraphData(graphKey);
        }

        public IStoredGraph Get(Guid internalId, IRegisteredGraph registeredGraph)
        {
            throw new NotImplementedException();
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

        private void updateIndexes(ITrackedGraph trackedGraph)
        {
            var graphKey = new DatabaseEntry(trackedGraph.InternalId.AsByteArray());

            foreach (var index in trackedGraph.Indexes
                .Select(index => new
                    {
                        keys = trackedGraph.ProjectedIndexes.Where(_ => _.IndexName == index.IndexName).Select(_ => _.UntypedKey), 
                        managedIndex = backingStore.IndexDatabases[index.IndexName]
                    }))
            {
                deleteAllIndexEntriesForGraphKey(index.managedIndex, graphKey);
            }

            insertIndexes(trackedGraph);
        }

        private void updateGraphData(ITrackedGraph trackedGraph)
        {
            backingStore.GraphDatabase.Put(
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()), 
                new DatabaseEntry(trackedGraph.SerialisedGraph.ToArray()), 
                transaction);
        }

        private void deleteAllIndexEntriesForGraphKey(ManagedIndex managedIndex, DatabaseEntry graphKey)
        {
            //Get all reverse index values for the internal id.
            var allKeysInIndex =
                managedIndex.ReverseIndex
                    .GetMultiple(graphKey, (int)managedIndex.ReverseIndex.Pagesize, transaction);

            Action<Cursor> actOnIndexEntry =
                cursor =>
                    {
                        if(cursor.Current.Value.Data.SequenceEqual(graphKey.Data))
                            cursor.Delete();
                    };

            //In the forward index, get all keys for each reverse index value and delete the record if the value matches 
            //the internal id of the graph we are deleting.
            onEachForwardIndexEntry(managedIndex, allKeysInIndex, actOnIndexEntry);

            managedIndex.ReverseIndex.Delete(graphKey, transaction);
        }

        private void onEachForwardIndexEntry(ManagedIndex managedIndex, KeyValuePair<DatabaseEntry, MultipleDatabaseEntry> reverseIndexKey, Action<Cursor> actOnIndexEntry)
        {
            foreach(var cursor in
                from indexKey in reverseIndexKey.Value
                let cursor = managedIndex.Index.Cursor(new CursorConfig(), transaction)
                where cursor.Move(indexKey, true)
                select cursor)
            {
                do
                    actOnIndexEntry(cursor);
                while(cursor.MoveNextDuplicate());

                cursor.Close();
            }
        }

        private void deleteConcreteType(DatabaseEntry graphKey)
        {
            backingStore.ConcreteTypeDatabase.Delete(graphKey, transaction);
        }

        private void deleteGraphData(DatabaseEntry graphKey)
        {
            backingStore.GraphDatabase.Delete(graphKey, transaction);
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
            foreach(var managedIndex in registeredGraph.Indexes.Select(index => backingStore.IndexDatabases[index.IndexName]))
            {
                deleteAllIndexEntriesForGraphKey(managedIndex, graphKey);
            }
        }

        private void deleteTypeHierarchy(DatabaseEntry graphKey)
        {
            var typeHierarchyDatabase = backingStore.IndexDatabases[backingStore.RegisteredTypeHierarchyIndex.IndexName];
            deleteAllIndexEntriesForGraphKey(typeHierarchyDatabase, graphKey);
        }

        private void insertConcreteType(ITrackedGraph trackedGraph)
        {
            backingStore.ConcreteTypeDatabase.PutNoOverwrite(
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                new DatabaseEntry(trackedGraph.GraphType.AsByteArray()),
                transaction);
        }

        private void insertGraphData(ITrackedGraph trackedGraph)
        {
            backingStore.GraphDatabase.PutNoOverwrite(
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                new DatabaseEntry(trackedGraph.SerialisedGraph.ToArray()),
                transaction);
        }

        private void insertIndexes(ITrackedGraph trackedGraph)
        {
            foreach(var each in 
                from projection in trackedGraph.ProjectedIndexes
                let managedIndex = backingStore.IndexDatabases[projection.IndexName]
                select new {projection, managedIndex})
            {
                each.managedIndex.Index
                    .Put(
                        new DatabaseEntry(each.managedIndex.PresentKeyAsByteArray(each.projection.UntypedKey)),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        transaction);
                each.managedIndex.ReverseIndex
                    .Put(
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        new DatabaseEntry(each.managedIndex.PresentKeyAsByteArray(each.projection.UntypedKey)),
                        transaction);
            }
        }

        private void insertTypeHierarchy(ITrackedGraph trackedGraph)
        {
            var typeHierarchyDatabase = backingStore.IndexDatabases[backingStore.RegisteredTypeHierarchyIndex.IndexName];

            foreach(var type in trackedGraph.TypeHierarchy)
            {
                typeHierarchyDatabase.Index
                    .Put(
                        new DatabaseEntry(type.FullName.AsByteArray()),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        transaction);
                typeHierarchyDatabase.ReverseIndex
                    .Put(
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        new DatabaseEntry(type.FullName.AsByteArray()),
                        transaction);
            }
        }
    }
}