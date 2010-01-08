namespace Stash.Specifications
{
    using System;

    public class DummyReduction : Reduction
    {
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public Func<TKey, TValue, TValue> Reduce<TKey, TValue>()
        {
            throw new NotImplementedException();
        }
    }
}