namespace Stash.Configuration
{
    using System;
    using Engine;

    /// <summary>
    /// The context for configuring a <see cref="Mapper{TGraph}"/>
    /// </summary>
    /// <typeparam name="TBackingStore"></typeparam>
    /// <typeparam name="TGraph"></typeparam>
    public class MapContext<TBackingStore, TGraph> where TBackingStore : BackingStore
    {
        public MapContext(RegisteredMapper<TGraph> registeredMapper)
        {
            RegisteredMapper = registeredMapper;
        }

        /// <summary>
        /// The configured mapper.
        /// </summary>
        public virtual RegisteredMapper<TGraph> RegisteredMapper { get; private set; }

        /// <summary>
        /// Instruct the configuration not to persist the map.
        /// The map is transient, and only required as an intermediate step prior for consumption by a <see cref="Reducer"/>.
        /// </summary>
        public virtual void DoNotPersist()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reduce the map with the given <paramref name="reducer"/>
        /// </summary>
        /// <param name="reducer"></param>
        public virtual void ReduceWith(Reducer reducer)
        {
            if(reducer == null) throw new ArgumentNullException("reducer");
            RegisteredMapper.RegisteredReducers.Add(new RegisteredReducer(reducer));
        }
    }
}