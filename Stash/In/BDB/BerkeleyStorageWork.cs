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
    using global::Stash.Configuration;

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

        public void DeleteGraph(Guid internalId, IRegisteredGraph registeredGraph)
        {
            BackingStore.GraphDatabase.Delete(new DatabaseEntry(internalId.AsByteArray()), Transaction);
            BackingStore.ConcreteTypeDatabase.Delete(new DatabaseEntry(internalId.AsByteArray()), Transaction);

            deleteIndexes(internalId, registeredGraph);
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
                new DatabaseEntry(trackedGraph.GraphType.AsByteArray()),
                Transaction);

            insertTypeHierarchy(trackedGraph);
            insertIndexes(trackedGraph);
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            throw new NotImplementedException();
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
        /// <param name="internalId"></param>
        /// <param name="registeredGraph"></param>
        private void deleteIndexes(Guid internalId, IRegisteredGraph registeredGraph)
        {
            foreach(var index in registeredGraph.Indexes)
            {
                var indexDatabase = BackingStore.IndexDatabases[index.IndexName];

                //Get all reverse index values for the internal id.
                var reverseIndexKey =
                    indexDatabase.ReverseIndex
                        .GetMultiple(
                            new DatabaseEntry(internalId.AsByteArray()),
                            (int)indexDatabase.ReverseIndex.Pagesize,
                            Transaction);

                //In the forward index, get all keys for each reverse index value and delete the record if the value matches 
                //the internal id of the graph we are deleting.
                foreach(var cursor in
                    from indexKey in reverseIndexKey.Value
                    let cursor = indexDatabase.Index.Cursor(new CursorConfig(), Transaction)
                    where cursor.Move(indexKey, true)
                    select cursor)
                {
                    do
                    {
                        if(cursor.Current.Value.Data.SequenceEqual(reverseIndexKey.Key.Data))
                            cursor.Delete();
                    } while(cursor.MoveNextDuplicate());
                }
            }
        }

        private void insertIndexes(ITrackedGraph trackedGraph)
        {
            foreach(var index in trackedGraph.ProjectedIndices)
            {
                var indexDatabase = BackingStore.IndexDatabases[index.IndexName];
                indexDatabase.Index
                    .Put(
                        new DatabaseEntry(indexDatabase.Config.AsByteArray(index.UntypedKey)),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        Transaction);
                indexDatabase.ReverseIndex
                    .Put(
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        new DatabaseEntry(indexDatabase.Config.AsByteArray(index.UntypedKey)),
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
                        new DatabaseEntry(type.FullName.AsByteArray()),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        Transaction);
                typeHierarchyDatabase.ReverseIndex
                    .Put(
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        new DatabaseEntry(type.FullName.AsByteArray()),
                        Transaction);
            }
        }
    }
}