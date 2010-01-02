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
            RegisteredIndexers = new List<RegisteredIndexer<TGraph>>();
            RegisteredMappers = new List<RegisteredMapper<TGraph>>();
        }

        public IList<RegisteredIndexer<TGraph>> RegisteredIndexers { get; private set; }
        public IList<RegisteredMapper<TGraph>> RegisteredMappers { get; private set; }
        public ISerializationSurrogate RegisteredSerializationSurrogate { get; set; }
    }
}