namespace Stash.Specifications.for_backingstore_bsb.given_managed_index
{
    using System;
    using System.Collections.Generic;

    public class NewIndex : IIndex<GraphAffectedByNewIndex,string>
    {
        public IEnumerable<string> Yield(GraphAffectedByNewIndex graph)
        {
            yield return graph.Text;
        }
    }
}