namespace Stash.In.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PersistentGraph
    {
        private readonly Func<Stream> fSerializedGraph;

        public PersistentGraph(Guid internalID, IEnumerable<Type> types, long version, Func<Stream> fSerializedGraph)
        {
            this.fSerializedGraph = fSerializedGraph;
            InternalId = internalID;
            Types = types;
            Version = version;
        }

        public Guid InternalId { get; private set; }
        public IEnumerable<Type> Types { get; private set; }
        public long Version { get; set; }

        public Stream SerializedGraph
        {
            get { return fSerializedGraph(); }
        }
    }
}