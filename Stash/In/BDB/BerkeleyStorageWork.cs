namespace Stash.In.BDB
{
    using System;
    using System.Linq;
    using BerkeleyDB;
    using Engine;

    public class BerkeleyStorageWork : IStorageWork
    {
        public BerkeleyBackingStore BackingStore { get; private set; }
        public Transaction Transaction { get; private set; }

        public BerkeleyStorageWork(BerkeleyBackingStore backingStore)
        {
            BackingStore = backingStore;
            Transaction = backingStore.Environment.BeginTransaction();
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

            foreach (var type in trackedGraph.SuperTypes)
            {
                BackingStore.TypeHierarchyDatabase.Put(
                    new DatabaseEntry(type.FullName.AsByteArray()),
                    new DatabaseEntry(trackedGraph.InternalId.AsByteArray()),
                    Transaction);
            }

            foreach (var index in trackedGraph.Indexes)
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

        public void DeleteGraph(Guid internalId)
        {
            throw new NotImplementedException();
        }

        public IStoredGraph Get(Guid internalId)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            Transaction.Commit();
        }
    }
}