namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;

    public interface IRegisteredGraph {
        /// <summary>
        /// The <see cref="Type"/> of the root of the object graph.
        /// </summary>
        Type GraphType { get; }

        /// <summary>
        /// The type hierarchy of the <see cref="GraphType"/>, including itself but excluding <see cref="object"/>.
        /// </summary>
        IEnumerable<Type> TypeHierarchy { get; }

        /// <summary>
        /// All registered indexes for the graph.
        /// </summary>
        IEnumerable<IRegisteredIndexer> Indexes { get; }
    }
}