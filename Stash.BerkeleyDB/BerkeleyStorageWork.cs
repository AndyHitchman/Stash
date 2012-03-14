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
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using BerkeleyQueries;
    using global::BerkeleyDB;
    using Stash.Configuration;
    using Stash.Engine;
    using Stash.Queries;

    public class BerkeleyStorageWork : IStorageWork
    {
        public BerkeleyStorageWork(BerkeleyBackingStore backingStore)
        {
            BackingStore = backingStore;
            Transaction = backingStore.Environment.DatabaseEnvironment.BeginTransaction();
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

        public void DeleteGraph(InternalId internalId, IRegisteredGraph registeredGraph)
        {
            var graphKey = new DatabaseEntry(internalId.AsByteArray());

            deleteIndexes(graphKey, registeredGraph);
            deleteTypeHierarchy(graphKey);
            deleteConcreteType(graphKey);
            deleteGraphData(graphKey);
        }

        /// <summary>
        /// Get executes the query in the given transaction and materialises the resulting internal ids immediately to
        /// prevent holding open cursors for a significant duration. However, the stored graphs are lazily yielded in
        /// distinct transactions.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            //This Get is lazy, so the transaction that originally did the match is likely closed. 
            //We go back to the backing store to start a new transaction.
            return Matching(query).Select(internalId => BackingStore.Get(internalId));
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            //Materialize the query operators in order to minimize the duration of open cursors.
            return executeQuery(query).Materialize();
        }

        public IStoredGraph Get(InternalId internalId)
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
            managedIndex.Delete(graphKey, Transaction);
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
            var typeHierarchyIndex = BackingStore.IndexDatabases[BackingStore.RegisteredTypeHierarchyIndex.IndexName];
            deleteAllIndexEntriesForGraphKey(typeHierarchyIndex, graphKey);
        }

        private IEnumerable<InternalId> executeQuery(IQuery query)
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
            foreach(var projection in trackedGraph.ProjectedIndexes)
                insertIndex((ProjectedIndex)projection, trackedGraph.InternalId);
        }

        private void insertIndex(ProjectedIndex projection, InternalId internalId)
        {
            var managedIndex = BackingStore.IndexDatabases[projection.IndexName];
            managedIndex.Insert(projection.UntypedKey, internalId, Transaction);
        }

        private void insertTypeHierarchy(ITrackedGraph trackedGraph)
        {
            var typeHierarchyIndex = BackingStore.IndexDatabases[BackingStore.RegisteredTypeHierarchyIndex.IndexName];

            foreach(var type in trackedGraph.TypeHierarchy)
            {
                typeHierarchyIndex.Insert(type, trackedGraph.InternalId, Transaction);
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
                            keys = trackedGraph.ProjectedIndexes.Where(_ => _.IndexName == index.IndexName).Cast<ProjectedIndex>().Select(_ => _.UntypedKey),
                            managedIndex = BackingStore.IndexDatabases[index.IndexName]
                        }))
            {
                deleteAllIndexEntriesForGraphKey(index.managedIndex, graphKey);
            }

            insertIndexes(trackedGraph);
        }
    }
}