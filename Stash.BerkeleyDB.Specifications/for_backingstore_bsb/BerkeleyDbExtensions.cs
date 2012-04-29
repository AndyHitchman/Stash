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

namespace Stash.BerkeleyDB.Specifications.for_backingstore_bsb
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using global::BerkeleyDB;
    using Serializers;
    using Stash.Engine;
    using NUnit.Framework;
    using Support;

    public static class BerkeleyDbExtensions
    {
        public static void ShouldHaveKey(this HashDatabase store, InternalId key)
        {
            store.Exists(new DatabaseEntry(key.AsByteArray())).ShouldBeTrue();
        }

        public static void ShouldHaveKey(this BTreeDatabase store, byte[] key)
        {
            store.Exists(new DatabaseEntry(key)).ShouldBeTrue();
        }

        public static void ShouldNotHaveKey(this HashDatabase store, InternalId key)
        {
            store.Exists(new DatabaseEntry(key.AsByteArray())).ShouldBeFalse();
        }

        public static void ShouldNotHaveKey(this BTreeDatabase store, byte[] key)
        {
            store.Exists(new DatabaseEntry(key)).ShouldBeFalse();
        }

        public static Stream AsStream(this byte[] bytes)
        {
            return new PreservedMemoryStream(bytes);
        }

        public static byte[] ValueForKey(this HashDatabase store, InternalId key)
        {
            try
            {
                return store.Get(new DatabaseEntry(key.AsByteArray())).Value.Data;
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForKey: Key not found");
            }
            return null;
        }

        public static byte[] ValueForKey(this BTreeDatabase store, object key)
        {
            return ValueForKey(store, key.ToString().AsByteArray());
        }

        public static byte[] ValueForKey(this BTreeDatabase store, byte[] key)
        {
            try
            {
                return store.Get(new DatabaseEntry(key)).Value.Data;
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForKey: Key not found");
            }
            return null;
        }

        public static IEnumerable<byte[]> ValuesForKey(this BTreeDatabase store, object key)
        {
            return ValuesForKey(store, key.ToString().AsByteArray());
        }

        public static IEnumerable<byte[]> ValuesForKey(this BTreeDatabase store, byte[] key)
        {
            try
            {
                return store.GetMultiple(new DatabaseEntry(key)).Value.Select(_ => _.Data);
            }
            catch(NotFoundException)
            {
                Assert.Fail("ValueForKey: Key not found");
            }
            return null;
        }
    }
}