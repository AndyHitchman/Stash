#region License
// Copyright 2009 Andrew Hitchman
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

namespace Stash.Specifications.for_backingstore_bsb.given_berkeley_backing_store
{
    using System.IO;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_instantiating : with_temp_dir
    {
        protected override void Given() {}

        protected override void When()
        {
            var create = Subject;
            Subject.Dispose();
        }

        [Then]
        public void it_should_create_the_directory_if_it_does_not_exist()
        {
            Directory.Exists(TempDir).ShouldBeTrue();
        }

        [Test]
        public void it_should_create_the_primary_database()
        {
            File.Exists(Path.Combine(TempDir, "data\\graphs.db")).ShouldBeTrue();
        }
    }
}