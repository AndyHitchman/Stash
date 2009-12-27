namespace Stash.Configuration
{
    using System;

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
        public Type AggregateType { get; private set; }
    }
}