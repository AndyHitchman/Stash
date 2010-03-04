#region License

// Copyright 2009 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.

#endregion

namespace Stash.Configuration
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using BackingStore;
    using Engine;

    /// <summary>
    /// The context for configuring a persistent object graph.
    /// </summary>
    /// <typeparam name="TBackingStore"></typeparam>
    /// <typeparam name="TGraph"></typeparam>
    public class GraphContext<TBackingStore, TGraph> where TBackingStore : IBackingStore where TGraph : class
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
        public virtual void IndexWith<TKey>(IIndex<TGraph, TKey> index) where TKey : IComparable<TKey>, IEquatable<TKey>
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
            var registeredMapper = new RegisteredMapper<TGraph, TKey, TValue>(map);
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