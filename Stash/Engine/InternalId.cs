namespace Stash.Engine
{
    using System;

    /// <summary>
    /// An internal id used by Stash to track and index graphs.
    /// </summary>
    [Serializable]
    public class InternalId
    {
        public Guid Value { get; private set; }

        public InternalId(Guid value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }


        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var otherInternalId = obj as InternalId;

            return !ReferenceEquals(otherInternalId, null) && Value.Equals(otherInternalId.Value);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public byte[] AsByteArray()
        {
            return Value.ToByteArray();
        }

        public static bool operator ==(InternalId lhs, InternalId rhs)
        {
            return ReferenceEquals(lhs, null) ? ReferenceEquals(rhs, null) : lhs.Equals(rhs);
        }

        public static bool operator !=(InternalId lhs, InternalId rhs)
        {
            return !(lhs == rhs);
        }
    }
}