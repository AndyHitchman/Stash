namespace Stash.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// A configured mapper.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class RegisteredMapper<TGraph>
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        public Mapper<TGraph> Mapper { get; set; }

        /// <summary>
        /// Reducers chained to the <see cref="Mapper"/>.
        /// </summary>
        public IEnumerable<Reducer> Reducers { get; set; }
    }
}