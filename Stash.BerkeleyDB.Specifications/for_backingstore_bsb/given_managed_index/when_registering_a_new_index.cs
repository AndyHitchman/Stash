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

namespace Stash.BerkeleyDB.Specifications.for_backingstore_bsb.given_managed_index
{
    using System.IO;
    using Stash.BerkeleyDB;
    using Stash.BerkeleyDB.BerkeleyConfigs;
    using Stash.Configuration;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_registering_a_new_index : with_backing_store_on_temp_dir
    {
        private RegisteredIndexer<ClassWithTwoAncestors, int> registeredIndexer;


        protected override void Given()
        {
            registeredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex(), registry);
        }

        protected override void When()
        {
            Subject.EnsureIndex(registeredIndexer);
        }

        [Then]
        public void it_should_create_the_index_database()
        {
            File.Exists(TempDir + "\\data\\" + ManagedIndex.IndexFilenamePrefix + registeredIndexer.IndexName + BerkeleyBackingStore.DatabaseFileExt).ShouldBeTrue
                ();
            File.Exists(TempDir + "\\data\\" + ManagedIndex.ReverseIndexFilenamePrefix + registeredIndexer.IndexName + BerkeleyBackingStore.DatabaseFileExt).
                ShouldBeTrue();
        }

        [Then]
        public void it_should_configure_the_database_with_the_correct_comparer()
        {
            Subject.IndexDatabases[registeredIndexer.IndexName].Index.Compare.Method.DeclaringType.ShouldEqual(typeof(IntIndexDatabaseConfig));
        }

    }
}