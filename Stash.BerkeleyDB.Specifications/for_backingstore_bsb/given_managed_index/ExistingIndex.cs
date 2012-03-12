namespace Stash.BerkeleyDB.Specifications.for_backingstore_bsb.given_managed_index
{
    using System.Collections.Generic;

    public class ExistingIndex : IIndex<GraphAffectedByNewIndex,string>
    {
        public IEnumerable<string> Yield(GraphAffectedByNewIndex graph)
        {
            yield return graph.Text;
        }
    }
}