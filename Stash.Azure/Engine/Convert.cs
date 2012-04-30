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

namespace Stash.Azure.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Convert
    {
        public class BiDiConvert
        {
            public Func<object, string> IntoString { get; set; }
            public Func<string, object> AsObject { get; set; }
        }

        public static readonly Dictionary<Type, BiDiConvert> For = new Dictionary<Type, BiDiConvert>
            {
                {typeof(string)     , new BiDiConvert {IntoString = o => ((string)o).AsString(), AsObject = s => s.AsString()}},
                {typeof(Guid)       , new BiDiConvert {IntoString = o => ((Guid)o).AsString(), AsObject = s => s.AsGuid()}},
                {typeof(Type)       , new BiDiConvert {IntoString = o => ((Type)o).AsString(), AsObject = s => s.AsType()}},
                {typeof(TimeSpan)   , new BiDiConvert {IntoString = o => ((TimeSpan)o).AsString(), AsObject = s => s.AsTimeSpan()}},
                {typeof(DateTime)   , new BiDiConvert {IntoString = o => ((DateTime)o).AsString(), AsObject = s => s.AsDateTime()}},
                {typeof(bool)       , new BiDiConvert {IntoString = o => ((bool)o).AsString(), AsObject = s => s.AsBoolean()}},
                {typeof(char)       , new BiDiConvert {IntoString = o => ((char)o).AsString(), AsObject = s => s.AsChar()}},
                {typeof(decimal)    , new BiDiConvert {IntoString = o => ((decimal)o).AsString(), AsObject = s => s.AsDecimal()}},
                {typeof(int)        , new BiDiConvert {IntoString = o => ((int)o).AsString(), AsObject = s => s.AsInt()}},
                {typeof(uint)       , new BiDiConvert {IntoString = o => ((uint)o).AsString(), AsObject = s => s.AsUInt()}},
                {typeof(long)       , new BiDiConvert {IntoString = o => ((long)o).AsString(), AsObject = s => s.AsLong()}},
                {typeof(ulong)      , new BiDiConvert {IntoString = o => ((ulong)o).AsString(), AsObject = s => s.AsULong()}},
                {typeof(short)      , new BiDiConvert {IntoString = o => ((short)o).AsString(), AsObject = s => s.AsShort()}},
                {typeof(ushort)     , new BiDiConvert {IntoString = o => ((ushort)o).AsString(), AsObject = s => s.AsUShort()}},
                {typeof(byte)       , new BiDiConvert {IntoString = o => ((byte)o).AsString(), AsObject = s => s.AsByte()}},
                {typeof(sbyte)      , new BiDiConvert {IntoString = o => ((sbyte)o).AsString(), AsObject = s => s.AsSByte()}},
            };


        public static string AsString(this string from)
        {
            return from;
        }

        public static string AsString(this Guid from)
        {
            return from.ToString();
        }

        public static Guid AsGuid(this string from)
        {
            return new Guid(from);
        }
        
        public static string AsString(this Type from)
        {
            return from.AssemblyQualifiedName.AsString();
        }

        public static Type AsType(this string from)
        {
            return Type.GetType(from);
        }
        
        public static string AsString(this TimeSpan from)
        {
            return from.Ticks.AsString();
        }

        public static TimeSpan AsTimeSpan(this string from)
        {
            return new TimeSpan(from.AsLong());
        }
        
        public static string AsString(this DateTime from)
        {
            return from.ToBinary().AsString();
        }

        public static DateTime AsDateTime(this string from)
        {
            return DateTime.FromBinary(from.AsLong());
        }

        public static string AsString(this bool from)
        {
            return from.ToString();
        }

        public static bool AsBoolean(this string from)
        {
            return bool.Parse(from);
        }

        public static string AsString(this char from)
        {
            return from.ToString();
        }

        public static char AsChar(this string from)
        {
            return from[0];
        }

        private const string decimalFormat = "00000000000000000000000000000.0000000000000000000000000000";

        public static string AsString(this decimal from)
        {
            var decimalString = from.ToString(decimalFormat);
            if (from < decimal.Zero)
                decimalString = complementDigits(decimalString);
            return decimalString;
        }

        public static decimal AsDecimal(this string from)
        {
            if (from[0] == '-')
                from = complementDigits(from);

            return decimal.Parse(from);
        }

        private const string intFormat = "0000000000";

        public static string AsString(this int from)
        {
            var intString = from.ToString(intFormat);
            if (from < 0)
                intString = complementDigits(intString);
            return intString;
        }

        public static int AsInt(this string from)
        {
            return int.Parse(from);
        }
        
        public static string AsString(this uint from)
        {
            return from.ToString(intFormat);
        }

        public static uint AsUInt(this string from)
        {
            return uint.Parse(from);
        }

        private const string longFormat = "0000000000000000000";

        public static string AsString(this long from)
        {
            return from.ToString(longFormat);
        }

        public static long AsLong(this string from)
        {
            return long.Parse(from);
        }

        public static string AsString(this ulong from)
        {
            return from.ToString(longFormat);
        }

        public static ulong AsULong(this string from)
        {
            return ulong.Parse(from);
        }

        private const string shortFormat = "00000";

        public static string AsString(this short from)
        {
            var shortString = from.ToString(shortFormat);
            if (from < 0)
                shortString = complementDigits(shortString);
            return shortString;
        }

        public static short AsShort(this string from)
        {
            return short.Parse(from);
        }

        public static string AsString(this ushort from)
        {
            return from.ToString(shortFormat);
        }

        public static ushort AsUShort(this string from)
        {
            return ushort.Parse(from);
        }

        private const string byteFormat = "000";

        public static string AsString(this sbyte from)
        {
            var byteString = from.ToString(byteFormat);
            if (from < 0)
                byteString = complementDigits(byteString);
            return byteString;
        }

        public static sbyte AsSByte(this string from)
        {
            return sbyte.Parse(from);
        }

        public static string AsString(this byte from)
        {
            return from.ToString(shortFormat);
        }

        public static byte AsByte(this string from)
        {
            return byte.Parse(from);
        }

        /// <summary>
        /// Negative numbers are complemented to ensure alphabetical sorting of string keys.
        /// </summary>
        /// <param name="decimalString"></param>
        /// <returns></returns>
        private static string complementDigits(string decimalString)
        {
            var complement = new Dictionary<char, char>
                {
                    {'.', '.'},
                    {'-', '-'},
                    {'9', '0'},
                    {'8', '1'},
                    {'7', '2'},
                    {'6', '3'},
                    {'5', '4'},
                    {'4', '5'},
                    {'3', '6'},
                    {'2', '7'},
                    {'1', '8'},
                    {'0', '9'},
                };
            return new string(decimalString.Select(c => complement[c]).ToArray());
        }
    }
}