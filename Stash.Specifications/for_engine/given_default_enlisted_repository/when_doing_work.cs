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

namespace Stash.Specifications.for_engine.given_default_enlisted_repository
{
    using Engine;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    [TestFixture]
    public class when_doing_work
    {
        [Test]
        public void it_should_delegate_delete_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<IInternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<IUnenlistedRepository>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.Delete(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Delete(mockSession, graph));
        }

        //        [Test]
        //        public void it_should_delegate_fetch_many_to_the_underlying_repository()
        //        {
        //            var mockSession = MockRepository.GenerateMock<InternalSession>();
        //            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
        //            var mockSelector = MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((IProjectedIndex<object>)null);
        //            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
        //            var expected = new[] {mockSelector};
        //
        //            sut.Fetch(expected);
        //
        //            mockUnenlistedRepository.AssertWasCalled(repository => repository.Fetch(mockSession, expected));
        //        }
        //
        //        [Test]
        //        public void it_should_delegate_fetch_to_the_underlying_repository()
        //        {
        //            var mockSession = MockRepository.GenerateMock<InternalSession>();
        //            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
        //            var mockSelector = MockRepository.GenerateMock<IFrom<DummyFrom, DummyPersistentObject>>();
        //            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
        //
        //            sut.Fetch(mockSelector);
        //
        //            mockUnenlistedRepository.AssertWasCalled(repository => repository.Fetch(mockSession, mockSelector));
        //        }

        [Test]
        public void it_should_delegate_get_tracker_for_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<IInternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<IUnenlistedRepository>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.GetTrackerFor(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.GetTrackerFor(mockSession, graph));
        }

        [Test]
        public void it_should_delegate_persist_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<IInternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<IUnenlistedRepository>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.Persist(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Persist(mockSession, graph));
        }

        [Test]
        public void it_should_delegate_reconnect_tracker_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<IInternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<IUnenlistedRepository>();
            var mockTracker = MockRepository.GenerateMock<Tracker>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.ReconnectTracker(mockTracker);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.ReconnectTracker(mockSession, mockTracker));
        }
    }
}