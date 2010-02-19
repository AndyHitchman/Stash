namespace Stash.Configuration
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Engine;

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

        public virtual IList<RegisteredIndexer<TGraph>> RegisteredIndexers { get; private set; }
        public virtual IList<RegisteredMapper<TGraph>> RegisteredMappers { get; private set; }

        public override void EngageBackingStore(IBackingStore backingStore)
        {
            foreach(var registeredIndexer in RegisteredIndexers)
                registeredIndexer.EngageBackingStore(backingStore);
        }
    }
}