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
            throw new NotImplementedException();
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
                new DatabaseEntry(trackedGraph.ConcreteType.FullName.AsByteArray()),
                Transaction);

            BackingStore.TypeHierarchyDatabase.Put(
                new DatabaseEntry(trackedGraph.ConcreteType.FullName.AsByteArray()),
                new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                Transaction);

            foreach(var type in trackedGraph.SuperTypes)
            {
                BackingStore.TypeHierarchyDatabase.Put(
                    new DatabaseEntry(type.FullName.AsByteArray()),
                    new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                    Transaction);
            }

            foreach(var index in trackedGraph.Indexes)
            {
                var indexDatabase = BackingStore.IndexDatabases[index.IndexName];
                indexDatabase.IndexDatabase
                    .Put(
                        new DatabaseEntry(indexDatabase.IndexDatabaseConfig.AsByteArray(index.UntypedKey)),
                        new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                        Transaction);
            }
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            throw new NotImplementedException();
        }
    }
}