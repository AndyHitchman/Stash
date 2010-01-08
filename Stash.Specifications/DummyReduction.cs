namespace Stash.Specifications
{
    using System;

    public class DummyReduction : Reduction<object, object>
    {
        public Func<object, object, object> Reduce()
        {
            throw new NotImplementedException();
        }
    }
}