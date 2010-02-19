namespace Stash.Configuration
{
    using System;
    using System.Runtime.Serialization;
    using Engine;

    /// <summary>
    /// An abstract configured graph.
    /// </summary>
    public abstract class RegisteredGraph
    {
        protected RegisteredGraph(Type aggregateType)
        {
            GraphType = aggregateType;
        }

        /// <summary>
        /// The <see cref="Type"/> of the root of the object graph.
        /// </summary>
        public virtual Type GraphType { get; private set; }

        public virtual ISerializationSurrogate RegisteredSerializationSurrogate { get; set; }


        public abstract void EngageBackingStore(IBackingStore backingStore);
    }
}