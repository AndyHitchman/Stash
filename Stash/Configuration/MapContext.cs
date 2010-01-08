namespace Stash.Configuration
{
    using System;
    using Engine;

    /// <summary>
    /// The context for configuring a <see cref="Map{TGraph}"/>
    /// </summary>
    /// <typeparam name="TBackingStore"></typeparam>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MapContext<TBackingStore, TGraph, TKey, TValue> where TBackingStore : BackingStore
    {
        public MapContext(RegisteredMapper<TGraph,TKey,TValue> registeredMapper)
        {
            RegisteredMapper = registeredMapper;
        }

        /// <summary>
        /// The configured Map.
        /// </summary>
        public virtual RegisteredMapper<TGraph,TKey,TValue> RegisteredMapper { get; private set; }

        /// <summary>
        /// Instruct the configuration not to persist the map.
        /// The map is transient, and only required as an intermediate step prior for consumption by a <see cref="Reduction"/>.
        /// </summary>
        public virtual void DoNotPersist()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reduce the map with the given <paramref name="reduction"/>
        /// </summary>
        /// <param name="reduction"></param>
        public virtual void ReduceWith(Reduction reduction)
        {
            if(reduction == null) throw new ArgumentNullException("reduction");
            RegisteredMapper.RegisteredReducers.Add(new RegisteredReducer(reduction));
        }
    }
}