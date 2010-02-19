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
                                                         new DatabaseEntry(trackedGraph.InternalId.ToByteArray()),
                                                         new DatabaseEntry(trackedGraph.SerialisedGraph.ToArray()),
                                                         Transaction);
            BackingStore.ConcreteTypeDatabase.PutNoOverwrite(
                                                                new DatabaseEntry(trackedGraph.InternalId.ToByteArray()),
                                                                new DatabaseEntry(trackedGraph.ConcreteType.ToStringAsByteArray()),
                                                                Transaction);

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