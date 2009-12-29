namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The context for configuring a <see cref="Mapper{TGraph}"/>
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class MapContext<TGraph>
    {
        public MapContext(RegisteredMapper<TGraph> registeredMapper)
        {
            RegisteredMapper = registeredMapper;
        }

        /// <summary>
        /// The configured mapper.
        /// </summary>
        public RegisteredMapper<TGraph> RegisteredMapper { get; private set; }

        /// <summary>
        /// Instruct the configuration not to persist the map.
        /// The map is transient, and only required for consumption by a <see cref="Reducer"/>.
        /// </summary>
        public void DoNotPersist()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reduce the map with the given <paramref name="reducer"/>
        /// </summary>
        /// <param name="reducer"></param>
        public void ReduceWith(Reducer reducer)
        {
            throw new NotImplementedException();
        }
    }
}