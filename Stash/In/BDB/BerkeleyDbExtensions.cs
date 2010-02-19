namespace Stash.In.BDB
{
    using System.Linq;

    public static class BerkeleyDbExtensions
    {
        public static byte[] ToStringAsByteArray(this object on)
        {
            if (on == null) return new byte[] {};

            return on.ToString().Select(_ => (byte)_).ToArray();
        }
    }
}