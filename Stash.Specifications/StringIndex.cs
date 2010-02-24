namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class StringIndex : Index<ClassWithTwoAncestors, string>
    {
        public IEnumerable<string> Yield(ClassWithTwoAncestors graph)
        {
            throw new NotImplementedException();
        }
    }
}