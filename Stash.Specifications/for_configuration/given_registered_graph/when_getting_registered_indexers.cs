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

namespace Stash.Specifications.for_configuration.given_registered_graph
{
    using Configuration;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    [TestFixture]
    public class when_getting_registered_indexers
    {
        [Test]
        public void it_should_find_a_registered_index_by_the_index_type()
        {
            var registeredIndexer = new RegisteredIndexer<DummyPersistentObject,int>(new DummyIndex());
            var sut = new RegisteredGraph<DummyPersistentObject>();
            sut.RegisteredIndexers.Add(registeredIndexer);

            sut.GetRegisteredIndexerFor<DummyIndex>().ShouldEqual(registeredIndexer);
        }

        [Test]
        public void it_should_find_the_correct_registered_index_by_the_index_type()
        {
            var registeredIndexer = new RegisteredIndexer<DummyPersistentObject,int>(new DummyIndex());
            var otherRegisteredIndexer = new RegisteredIndexer<DummyPersistentObject,int>(new OtherDummyIndex());
            var sut = new RegisteredGraph<DummyPersistentObject>();
            sut.RegisteredIndexers.Add(registeredIndexer);
            sut.RegisteredIndexers.Add(otherRegisteredIndexer);

            sut.GetRegisteredIndexerFor<DummyIndex>().ShouldEqual(registeredIndexer);
        }
    }
}