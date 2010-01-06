namespace Stash.Configuration
{
    using System;
    using Engine;

    /// <summary>
    /// An abstract configured graph.
    /// </summary>
    public abstract class RegisteredGraph
    {
        protected RegisteredGraph(Type aggregateType)
        {
            AggregateType = aggregateType;
        }

        /// <summary>
        /// The <see cref="Type"/> of the root of the object graph.
        /// </summary>
        public virtual Type AggregateType { get; private set; }


        public abstract void EngageBackingStore(BackingStore backingStore);
    }
}