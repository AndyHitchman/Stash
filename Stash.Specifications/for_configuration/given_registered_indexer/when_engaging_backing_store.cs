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

namespace Stash.Specifications.for_configuration.given_registered_indexer
{
    using NUnit.Framework;

    [TestFixture]
    public class when_engaging_backing_store
    {
        //        [Test]
        //        public void it_should_tell_the_backing_store_to_ensure_the_indexer_is_managed()
        //        {
        //            var mockBackingStore = MockRepository.GenerateMock<IBackingStore>();
        //            var fakeIndexer = MockRepository.GenerateStub<Index<DummyPersistentObject, object>>();
        //            var sut = new RegisteredIndexer<DummyPersistentObject, object>(fakeIndexer);
        //
        //            sut.EngageBackingStore(mockBackingStore);
        //
        //            mockBackingStore.AssertWasCalled(backingStore => backingStore.EnsureIndex(fakeIndexer));
        //        }
    }
}