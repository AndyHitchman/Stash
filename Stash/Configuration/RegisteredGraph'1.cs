namespace Stash.Configuration
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A configured object graph.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class RegisteredGraph<TGraph> : RegisteredGraph
    {
        public RegisteredGraph() : base(typeof(TGraph))
        {
        }

        public IEnumerable<Indexer<TGraph>> Indexers { get; set; }
        public IEnumerable<RegisteredMapper<TGraph>> ConfiguredMappers { get; set; }
        public ISerializationSurrogate ConfiguredSerializationSurrogate { get; set; }
    }
}