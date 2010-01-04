namespace Stash.In.BDB
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Engine;

    /// <summary>
    /// A backing store for Stash using BerkeleyDB.
    /// </summary>
    public class BerkeleyBackingStore : BackingStore
    {
        private readonly FileInfo databasePath;

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        /// <param name="databasePath"></param>
        public BerkeleyBackingStore(FileInfo databasePath)
        {
            if(!databasePath.Exists)
            {
            }
        }

        /// <summary>
        /// Create an instance of the backing store implementation using BerkeleyDB
        /// </summary>
        /// <param name="databasePath"></param>
        public BerkeleyBackingStore(string databasePath)
            : this(new FileInfo(databasePath))
        {
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void InsertGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        public void UpdateGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }

        public void DeleteGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersistentGraph> GetGraphs(IEnumerable<Guid> internalIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersistentGraph> GetExternallyModifiedGraphs(IEnumerable<PersistentGraph> persistentGraphs)
        {
            throw new NotImplementedException();
        }
    }
}