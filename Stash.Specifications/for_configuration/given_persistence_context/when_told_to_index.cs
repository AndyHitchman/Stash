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

namespace Stash.Specifications.for_configuration.given_persistence_context
{
    using System;
    using Configuration;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_told_to_index : with_persistence_context
    {
        [Test]
        public void it_should_complain_if_the_indexer_is_null()
        {
            IIndex<DummyPersistentObject, int> expected = null;

            typeof(ArgumentNullException)
                .ShouldBeThrownBy(() => Sut.Index(expected));
        }

        [Test]
        public void it_should_register_the_indexer()
        {
            var expected = new DummyIndex();
            Sut.Index(expected);
            Sut.Registry.RegisteredIndexers.ShouldContain(
                indexer => ((RegisteredIndexer<DummyPersistentObject, int>)indexer).Index == expected);
        }
    }
}