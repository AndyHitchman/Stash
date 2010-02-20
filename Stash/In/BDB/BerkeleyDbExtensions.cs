namespace Stash.In.BDB
{
    using System.Linq;

    public static class BerkeleyDbExtensions
    {
        public static byte[] ToByteArray(this string on)
        {
            if(on == null) return new byte[] {};

            return on.Select(_ => (byte)_).ToArray();
        }
    }
}