namespace Stash.Configuration
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Engine;

    /// <summary>
    /// A configured object graph.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class RegisteredGraph<TGraph> : RegisteredGraph where TGraph : class
    {
        public RegisteredGraph() : base(typeof(TGraph))
        {
            RegisteredIndexers = new List<RegisteredIndexer<TGraph>>();
            RegisteredMappers = new List<RegisteredMapper<TGraph>>();
        }

        public virtual IList<RegisteredIndexer<TGraph>> RegisteredIndexers { get; private set; }
        public virtual IList<RegisteredMapper<TGraph>> RegisteredMappers { get; private set; }
        public virtual ISerializationSurrogate RegisteredSerializationSurrogate { get; set; }

        public override void EngageBackingStore(BackingStore backingStore)
        {
            foreach(var registeredIndexer in RegisteredIndexers)
                registeredIndexer.EngageBackingStore(backingStore);
        }
    }
}