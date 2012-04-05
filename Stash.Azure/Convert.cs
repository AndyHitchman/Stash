#region License
// Copyright 2009, 2010 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

namespace Stash.Azure
{
    using System;
    using System.Collections.Generic;

    public static class Convert
    {
        private const string decimalFormat = "00000000000000000000000000000.0000000000000000000000000000";
        private const string doubleFormat = "00000000000000000.00000000000000000E+000";
        private const string longFormat = "0000000000000000000";

        public static readonly Dictionary<Type,Func<object,string>> IntoString = new Dictionary<Type, Func<object, string>>
            {
                {typeof(string)     , o => ((string)o).AsString()},
                {typeof(Type)       , o => ((Type)o).AsString()},
                {typeof(TimeSpan)   , o => ((TimeSpan)o).AsString()},
                {typeof(DateTime)   , o => ((DateTime)o).AsString()},
                {typeof(bool)       , o => ((bool)o).AsString()},
                {typeof(char)       , o => ((char)o).AsString()},
                {typeof(decimal)    , o => ((decimal)o).AsString()},
//                {typeof(double)     , o => ((double)o).AsString()},
//                {typeof(float)      , o => ((float)o).AsString()},
//                {typeof(int)        , o => ((int)o).AsString()},
//                {typeof(long)       , o => ((long)o).AsString()},
//                {typeof(short)      , o => ((short)o).AsString()},
//                {typeof(uint)       , o => ((uint)o).AsString()},
//                {typeof(ulong)      , o => ((ulong)o).AsString()},
//                {typeof(ushort)     , o => ((ushort)o).AsString()},
                {typeof(string)     , o => ((string)o).AsString()},
                {typeof(string)     , o => ((string)o).AsString()},
            };


        public static bool AsBoolean(this string from)
        {
            return bool.Parse(from);
        }

        public static string AsString(this string from)
        {
            return from;
        }

        public static string AsString(this Type from)
        {
            return from.AssemblyQualifiedName.AsString();
        }

        public static string AsString(this TimeSpan from)
        {
            return from.Ticks.AsString();
        }

        public static string AsString(this DateTime from)
        {
            return from.ToBinary().AsString();
        }

        public static string AsString(this bool from)
        {
            return from.ToString();
        }

        public static string AsString(this char from)
        {
            return from.ToString();
        }

        public static string AsString(this decimal from)
        {
            return
                from >= decimal.Zero
                    ? from.ToString(decimalFormat)
                    : (decimal.MinValue - from - 1).ToString(decimalFormat);
        }

//        public static string AsString(this double from)
//        {
//            return 
//                from >= 0.0
//                    ? from.ToString()
//            return BitConverter.GetBytes(from);
//        }
//
//        public static string AsString(this float from)
//        {
//            return BitConverter.GetBytes(from);
//        }
//
//        public static string AsString(this int from)
//        {
//            return BitConverter.GetBytes(from);
//        }
//
        public static string AsString(this long from)
        {
            return from.ToString(longFormat);
        }

//        public static string AsString(this short from)
//        {
//            return BitConverter.GetBytes(from);
//        }
//
//        public static string AsString(this uint from)
//        {
//            return BitConverter.GetBytes(from);
//        }
//
//        public static string AsString(this ulong from)
//        {
//            return BitConverter.GetBytes(from);
//        }
//
//        public static string AsString(this ushort from)
//        {
//            return BitConverter.GetBytes(from);
//        }
//
//        public static char AsChar(this string from)
//        {
//            return BitConverter.ToChar(from, 0);
//        }
//
//        public static DateTime AsDateTime(this string from)
//        {
//            return DateTime.FromBinary(from.AsLong());
//        }
//
//        public static decimal AsDecimal(this string from)
//        {
//            return
//                new decimal(
//                    new[]
//                        {
//                            from.asInt(0),
//                            from.asInt(4),
//                            from.asInt(8),
//                            from.asInt(12),
//                        });
//        }
//
//        public static double AsDouble(this string from)
//        {
//            return BitConverter.ToDouble(from, 0);
//        }
//
//        public static float AsFloat(this string from)
//        {
//            return BitConverter.ToSingle(from, 0);
//        }
//
//        public static InternalId AsInternalId(this string from)
//        {
//            return new InternalId(new Guid(from));
//        }
//
//        public static Guid AsGuid(this string from)
//        {
//            return new Guid(from);
//        }
//
//        public static int AsInt(this string from)
//        {
//            return BitConverter.ToInt32(from, 0);
//        }
//
//        public static long AsLong(this string from)
//        {
//            return BitConverter.ToInt64(from, 0);
//        }
//
//        public static short AsShort(this string from)
//        {
//            return BitConverter.ToInt16(from, 0);
//        }
//
//        public static TimeSpan AsTimeSpan(this string from)
//        {
//            return new TimeSpan(from.AsLong());
//        }
//
//        public static Type AsType(this string from)
//        {
//            if(from == null) return null;
//
//            return Type.GetType(new string(from.Select(_ => (char)_).ToArray()));
//        }
//
//        public static uint AsUInt(this string from)
//        {
//            return BitConverter.ToUInt32(from, 0);
//        }
//
//        public static ulong AsULong(this string from)
//        {
//            return BitConverter.ToUInt64(from, 0);
//        }
//
//        public static ushort AsUShort(this string from)
//        {
//            return BitConverter.ToUInt16(from, 0);
//        }
//
//        private static int asInt(this string from, int index)
//        {
//            return BitConverter.ToInt32(from, index);
//        }
    }
}