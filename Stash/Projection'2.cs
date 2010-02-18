namespace Stash
{
    using System;

    /// <summary>
    /// The result of a projection.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Projection<TKey, TValue> : Projection
    {
        public Projection(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public override object UntypedKey
        {
            get { return Key; }
        }

        public override object UntypedValue
        {
            get { return Value; }
        }
    }
}