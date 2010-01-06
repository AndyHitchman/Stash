namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PersistentGraph
    {
        private readonly Func<Stream> fSerializedGraph;

        /// <summary>
        /// Manage a transient persistent graph. A new <see cref="InternalId"/> is created. <see cref="Version"/> is zero.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="fSerializedGraph"></param>
        public PersistentGraph(IEnumerable<Type> types, Func<Stream> fSerializedGraph) : this(Guid.NewGuid(), types, 0L, fSerializedGraph)
        {
        }

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

        public virtual Guid InternalId { get; private set; }
        public virtual IEnumerable<Type> Types { get; private set; }
        public virtual long Version { get; private set; }

        public virtual void ActOnSerializedGraph(Action<Stream> action)
        {
            using(var stream = fSerializedGraph())
            {
                action(stream);
            }
        }
    }
}