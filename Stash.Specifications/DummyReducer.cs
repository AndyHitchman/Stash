namespace Stash.Specifications
{
    using System;
    using System.Collections.Generic;

    public class DummyReducer : Reducer
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