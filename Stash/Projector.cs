namespace Stash
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A projector is a mechanism to emit an alternative view of an aggregrate.
    /// </summary>
    public interface Projector
    {
        /// <summary>
        /// The name of the mapper. Must conform to naming rules for the persistence store.
        /// </summary>
        string Name { get; }
    }
}