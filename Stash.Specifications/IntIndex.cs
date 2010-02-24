namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class IntIndex : Index<ClassWithTwoAncestors, int>
    {
        public IEnumerable<int> Yield(ClassWithTwoAncestors graph)
        {
            throw new NotImplementedException();
        }
    }
}