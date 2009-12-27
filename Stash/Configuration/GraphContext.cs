namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Engine;

    /// <summary>
    /// The context for configuring a persistent object graph.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class GraphContext<TGraph>
    {
        /// <summary>
        /// The configured object graph.
        /// </summary>
        public RegisteredGraph<TGraph> RegisteredGraph { get; private set; }

        /// <summary>
        /// Index the object graph with the given <paramref name="indexer"/>.
        /// </summary>
        /// <param name="indexer"></param>
        public void IndexWith(Indexer<TGraph> indexer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Map the object graph with the given <paramref name="mapper"/>
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public MapContext<TGraph> MapWith(Mapper<TGraph> mapper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tell the engine to use the provided serializaton surrogate rather than the default <see cref="StashSerializationSurrogate"/>.
        /// </summary>
        /// <param name="surrogate"></param>
        public void SerializeWith(ISerializationSurrogate surrogate)
        {
            throw new NotImplementedException();
        }
    }
}