namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PersistentGraph
    {
        private readonly Func<Stream> fSerializedGraph;

        /// <summary>
        /// Manage a persistent graph.
        /// </summary>
        /// <param name="internalID"></param>
        /// <param name="types"></param>
        /// <param name="version"></param>
        /// <param name="fSerializedGraph"></param>
        public PersistentGraph(Guid internalID, IEnumerable<Type> types, long version, Func<Stream> fSerializedGraph)
        {
            this.fSerializedGraph = fSerializedGraph;
            InternalId = internalID;
            Types = types;
            Version = version;
        }

        public Guid InternalId { get; private set; }
        public IEnumerable<Type> Types { get; private set; }
        public long Version { get; private set; }

        public void ActOnSerializedGraph(Action<Stream> action)
        {
            using(var stream = fSerializedGraph())
            {
                action(stream);
            }
        }
    }
}