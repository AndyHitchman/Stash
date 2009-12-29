namespace Stash.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// A configured mapper.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class RegisteredMapper<TGraph>
    {
        public RegisteredMapper(Mapper<TGraph> mapper)
        {
            Mapper = mapper;
            RegisteredReducers = new List<Reducer>();
        }

        /// <summary>
        /// The mapper.
        /// </summary>
        public Mapper<TGraph> Mapper { get; private set; }

        /// <summary>
        /// Reducers chained to the <see cref="Mapper"/>.
        /// </summary>
        public IList<Reducer> RegisteredReducers { get; private set; }
    }
}