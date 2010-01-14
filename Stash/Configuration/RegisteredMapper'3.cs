namespace Stash.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Engine;

    /// <summary>
    /// A configured Map.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class RegisteredMapper<TGraph, TKey, TValue> : RegisteredMapper<TGraph> where TGraph : class
    {
        public RegisteredMapper(Map<TGraph,TKey,TValue> map)
        {
            Map = map;
            RegisteredReducers = new List<RegisteredReducer<TKey, TValue>>();
        }

        /// <summary>
        /// The Map.
        /// </summary>
        public Map<TGraph,TKey,TValue> Map { get; private set; }

        /// <summary>
        /// Reducers chained to the <see cref="Map"/>.
        /// </summary>
        public IList<RegisteredReducer<TKey, TValue>> RegisteredReducers { get; private set; }

        public override void EngageBackingStore(BackingStore backingStore)
        {
            backingStore.EnsureMap(Map);
        }

        public override IEnumerable<Projection<TGraph>> GetKeyFreeProjections(TGraph graph)
        {
            return Map.F(graph).Cast<Projection<TGraph>>();
        }

    }
}