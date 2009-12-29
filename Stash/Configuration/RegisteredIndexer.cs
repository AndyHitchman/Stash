namespace Stash.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// A configured indexer.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public class RegisteredIndexer<TGraph>
    {
        public RegisteredIndexer(Indexer<TGraph> indexder)
        {
            Indexer = indexder;
        }

        /// <summary>
        /// The mapper.
        /// </summary>
        public Indexer<TGraph> Indexer { get; private set; }
    }
}