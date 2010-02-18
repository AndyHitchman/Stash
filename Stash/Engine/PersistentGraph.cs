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
        /// <param name="mapProjections"></param>
        /// <param name="fSerializedGraph"></param>
        /// <param name="indexProjections"></param>
        public PersistentGraph(
            IEnumerable<Type> types, 
            IDictionary<string, List<TrackedProjection>> indexProjections,
            IDictionary<string, List<TrackedProjection>> mapProjections,
            Func<Stream> fSerializedGraph) 
            : this(Guid.NewGuid(), types, indexProjections, mapProjections, 0L, fSerializedGraph)
        {
        }

        /// <summary>
        /// Manage a persistent graph.
        /// </summary>
        /// <param name="internalID"></param>
        /// <param name="types"></param>
        /// <param name="mapProjections"></param>
        /// <param name="version"></param>
        /// <param name="fSerializedGraph"></param>
        /// <param name="indexProjections"></param>
        public PersistentGraph(
            Guid internalID, 
            IEnumerable<Type> types,
            IDictionary<string, List<TrackedProjection>> indexProjections,
            IDictionary<string, List<TrackedProjection>> mapProjections,
            long version, 
            Func<Stream> fSerializedGraph)
        {
            this.fSerializedGraph = fSerializedGraph;
            InternalId = internalID;
            Types = types;
            IndexProjections = indexProjections;
            MapProjections = mapProjections;
            Version = version;
        }

        public virtual Guid InternalId { get; private set; }
        public virtual IEnumerable<Type> Types { get; private set; }
        public virtual IDictionary<string, List<TrackedProjection>> IndexProjections { get; private set; }
        public virtual IDictionary<string, List<TrackedProjection>> MapProjections { get; private set; }
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