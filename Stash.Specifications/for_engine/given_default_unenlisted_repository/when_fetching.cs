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

namespace Stash.Specifications.for_engine.given_default_unenlisted_repository
{
    using NUnit.Framework;

    [TestFixture]
    public class when_fetching
    {
        //        [Test, Ignore("Isn't valid: Can fetch maps and reductions")]
        //        public void it_should_ensure_that_the_registry_is_managing_the_graph_type_for_a_graph_fetch()
        //        {
        //            var mockSession = MockRepository.GenerateMock<InternalSession>();
        //            var mockRegistry = MockRepository.GenerateMock<Registry>();
        //            var mockSelector =
        //                MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((IProjectedIndex<object>)null);
        //            var sut = new DefaultUnenlistedRepository();
        //            var graph = new DummyPersistentObject();
        //
        //            mockSession.Expect(s => s.Registry).Return(mockRegistry);
        //            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(typeof(DummyPersistentObject))).Return(false);
        //
        //            typeof(ArgumentOutOfRangeException).ShouldBeThrownBy(
        //                () => sut.Fetch(mockSession, new[] {mockSelector}));
        //        }
        //
        //        [Test, Ignore("Isn't valid: Can fetch maps and reductions")]
        //        public void it_should_ensure_that_the_registry_is_managing_the_graph_type_for_a_projected_fetch()
        //        {
        //            var mockSession = MockRepository.GenerateMock<InternalSession>();
        //            var mockRegistry = MockRepository.GenerateMock<Registry>();
        //            var mockSelector =
        //                MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((IProjectedIndex<object>)null);
        //            var sut = new DefaultUnenlistedRepository();
        //            var graph = new DummyPersistentObject();
        //
        //            mockSession.Expect(s => s.Registry).Return(mockRegistry);
        //            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(typeof(DummyPersistentObject))).Return(false);
        //
        //            typeof(ArgumentOutOfRangeException).ShouldBeThrownBy(
        //                () => sut.Fetch(mockSession, mockSelector));
        //        }
        //
        //        [Test]
        //        public void it_should_give_the_session_a_persistence_track_for_each_graph()
        //        {
        //            var mockSession = MockRepository.GenerateMock<InternalSession>();
        //            var mockRegistry = MockRepository.GenerateMock<Registry>();
        //            var mockPersistenceEventFactory = MockRepository.GenerateMock<PersistenceEventFactory>();
        //            var mockSelector =
        //                MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((IProjectedIndex<object>)null);
        //            var mockTrack = MockRepository.GenerateStub<Track<DummyPersistentObject>>(Guid.Empty, null, Stream.Null, null);
        //            var sut = new DefaultUnenlistedRepository();
        //
        //            mockSession.Expect(s => s.Registry).Return(mockRegistry);
        //            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(null)).IgnoreArguments().Return(true);
        //            mockSession.Expect(_ => _.PersistenceEventFactory).Return(mockPersistenceEventFactory);
        //            mockPersistenceEventFactory.Expect(_ => _.MakeTrack<DummyPersistentObject>(Guid.Empty, null, null, null)).IgnoreArguments().Return(mockTrack);
        //
        //            sut.Fetch(mockSession, new[] {mockSelector});
        //
        //            mockPersistenceEventFactory.VerifyAllExpectations();
        //        }
        //
        //        [Test]
        //        public void it_should_give_the_session_a_persistence_track_for_each_projection()
        //        {
        //            var mockSession = MockRepository.GenerateMock<InternalSession>();
        //            var mockRegistry = MockRepository.GenerateMock<Registry>();
        //            var mockPersistenceEventFactory = MockRepository.GenerateMock<PersistenceEventFactory>();
        //            var mockSelector =
        //                MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((IProjectedIndex<object>)null);
        //            var mockRegisteredGraph = MockRepository.GenerateMock<RegisteredGraph<DummyPersistentObject>>();
        //            var mockTrack = MockRepository.GenerateStub<Track<DummyPersistentObject>>(Guid.Empty, null, Stream.Null, null);
        //            var sut = new DefaultUnenlistedRepository();
        //
        //            mockSession.Expect(s => s.Registry).Return(mockRegistry);
        //            mockRegistry.Expect(r => r.IsManagingGraphTypeOrAncestor(null)).IgnoreArguments().Return(true);
        //            mockSession.Expect(_ => _.PersistenceEventFactory).Return(mockPersistenceEventFactory);
        //            mockPersistenceEventFactory.Expect(_ => _.MakeTrack<DummyPersistentObject>(Guid.Empty, null, null, null)).IgnoreArguments().Return(mockTrack);
        //
        //            sut.Fetch(mockSession, mockSelector);
        //
        //            mockPersistenceEventFactory.VerifyAllExpectations();
        //        }
    }
}