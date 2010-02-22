namespace Stash.In.BDB
{
    using System;
    using System.Linq;

    public static class Convert
    {
        public static byte[] AsByteArray(this string from)
        {
            if (from == null) return new byte[] { };

            return from.Select(_ => (byte)_).ToArray();
        }

        public static string AsString(this byte[] from)
        {
            if (from == null) return null;

            return new string(from.Select(_ => (char)_).ToArray());
        }

        public static byte[] AsByteArray(this Type from)
        {
            return from.FullName.AsByteArray();
        }

        public static Type AsType(this byte[] from)
        {
            if (from == null) return null;

            return Type.GetType(new string(from.Select(_ => (char)_).ToArray()));
        }

        public static byte[] AsByteArray(this Guid from)
        {
            return from.ToByteArray();
        }

        public static Guid AsGuid(this byte[] from)
        {
            return new Guid(from);
        }

        public static byte[] AsByteArray(this TimeSpan from)
        {
            return from.Ticks.AsByteArray();
        }

        public static TimeSpan AsTimeSpan(this byte[] from)
        {
            return new TimeSpan(from.AsLong());
        }

        public static byte[] AsByteArray(this DateTime from)
        {
            return from.ToBinary().AsByteArray();
        }

        public static DateTime AsDateTime(this byte[] from)
        {
            return DateTime.FromBinary(from.AsLong());
        }

        public static byte[] AsByteArray(this bool from)
        {
            return BitConverter.GetBytes(from);
        }

        public static bool AsBoolean(this byte[] from)
        {
            return BitConverter.ToBoolean(from, 0);
        }

        public static byte[] AsByteArray(this char from)
        {
            return BitConverter.GetBytes(from);
        }

        public static char AsChar(this byte[] from)
        {
            return BitConverter.ToChar(from, 0);
        }

        public static byte[] AsByteArray(this decimal from)
        {
            return decimal.GetBits(from).SelectMany(i => i.AsByteArray()).ToArray();
        }

        public static decimal AsDecimal(this byte[] from)
        {
            return
                new decimal(
                    new[]
                        {
                            from.asInt(0),
                            from.asInt(4),
                            from.asInt(8),
                            from.asInt(12),
                        });
        }

        public static byte[] AsByteArray(this double from)
        {
            return BitConverter.GetBytes(from);
        }

        public static double AsDouble(this byte[] from)
        {
            return BitConverter.ToDouble(from, 0);
        }

        public static byte[] AsByteArray(this float from)
        {
            return BitConverter.GetBytes(from);
        }

        public static float AsFloat(this byte[] from)
        {
            return BitConverter.ToSingle(from, 0);
        }

        public static byte[] AsByteArray(this int from)
        {
            return BitConverter.GetBytes(from);
        }

        public static int AsInt(this byte[] from)
        {
            return BitConverter.ToInt32(from, 0);
        }

        private static int asInt(this byte[] from, int index)
        {
            return BitConverter.ToInt32(from, index);
        }

        public static byte[] AsByteArray(this long from)
        {
            return BitConverter.GetBytes(from);
        }

        public static long AsLong(this byte[] from)
        {
            return BitConverter.ToInt64(from, 0);
        }

        public static byte[] AsByteArray(this short from)
        {
            return BitConverter.GetBytes(from);
        }

        public static short AsShort(this byte[] from)
        {
            return BitConverter.ToInt16(from, 0);
        }

        public static byte[] AsByteArray(this uint from)
        {
            return BitConverter.GetBytes(from);
        }

        public static uint AsUInt(this byte[] from)
        {
            return BitConverter.ToUInt32(from, 0);
        }

        public static byte[] AsByteArray(this ulong from)
        {
            return BitConverter.GetBytes(from);
        }

        public static ulong AsULong(this byte[] from)
        {
            return BitConverter.ToUInt64(from, 0);
        }

        public static byte[] AsByteArray(this ushort from)
        {
            return BitConverter.GetBytes(from);
        }

        public static ushort AsUShort(this byte[] from)
        {
            return BitConverter.ToUInt16(from, 0);
        }

    }
}