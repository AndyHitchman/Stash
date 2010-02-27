namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class IntIndex : IIndex<ClassWithTwoAncestors, int>
    {
        public IEnumerable<int> Yield(ClassWithTwoAncestors graph)
        {
            throw new NotImplementedException();
        }
    }
}