namespace Stash.Configuration
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using Engine;

    /// <summary>
    /// The context for configuring a persistent object graph.
    /// </summary>
    /// <typeparam name="TBackingStore"></typeparam>
    /// <typeparam name="TGraph"></typeparam>
    public class GraphContext<TBackingStore, TGraph> where TBackingStore : BackingStore where TGraph : class
    {
        public GraphContext(RegisteredGraph<TGraph> registeredGraph)
        {
            RegisteredGraph = registeredGraph;
        }

        /// <summary>
        /// The configured object graph.
        /// </summary>
        public virtual RegisteredGraph<TGraph> RegisteredGraph { get; private set; }

        /// <summary>
        /// Index the object graph with the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <typeparam name="TKey"></typeparam>
        public virtual void IndexWith<TKey>(Index<TGraph, TKey> index)
        {
            if(index == null) throw new ArgumentNullException("index");
            var registeredIndexer = new RegisteredIndexer<TGraph, TKey>(index);
            RegisteredGraph.RegisteredIndexers.Add(registeredIndexer);
        }

        /// <summary>
        /// Map the object graph with the given <paramref name="map"/>
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public virtual MapContext<TBackingStore, TGraph, TKey, TValue> MapWith<TKey, TValue>(Map<TGraph, TKey, TValue> map)
        {
            if(map == null) throw new ArgumentNullException("map");
            var registeredMapper = new RegisteredMapper<TGraph,TKey,TValue>(map);
            RegisteredGraph.RegisteredMappers.Add(registeredMapper);
            return new MapContext<TBackingStore, TGraph, TKey, TValue>(registeredMapper);
        }

        /// <summary>
        /// Tell the engine to use the provided serializaton functions.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="deserializer"></param>
        public virtual void SerializeWith(Func<TGraph, Stream> serializer, Func<Stream, TGraph> deserializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tell the engine to use the provided serializaton functions implemented by the <paramref name="customSerializer"/>.
        /// </summary>
        /// <param name="customSerializer"></param>
        public virtual void SerializeWith(CustomSerializer customSerializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tell the engine to use the provided serializaton surrogate rather than the default <see cref="StashSerializationSurrogate"/>.
        /// </summary>
        /// <param name="surrogate"></param>
        public virtual void SerializeWith(ISerializationSurrogate surrogate)
        {
            throw new NotImplementedException();
        }
    }
}