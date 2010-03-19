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

namespace Stash.Specifications.for_stashed_set
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using Queries;
    using Rhino.Mocks;
    using Support;

    public class when_getting_a_stashed_set : Specification
    {
        private IEnumerator<DummyPersistentObject> actual;
        private IInternalSession mockInternalSession;
        private ITrack<DummyPersistentObject> mockTrack;
        private StashedSet<DummyPersistentObject> subject;
        private IStoredGraph mockStoredGraph;
        private IRegisteredGraph<DummyPersistentObject> mockRegisteredGraph;

        protected override void Given()
        {
            mockInternalSession = MockRepository.GenerateMock<IInternalSession>();
            var mockRegistry = MockRepository.GenerateStub<IRegistry>();
            var mockBackingStore = MockRepository.GenerateStub<IBackingStore>();
            var mockQueryFactory = MockRepository.GenerateStub<IQueryFactory>();
            var mockQuery = MockRepository.GenerateStub<IQuery>();

            subject = new StashedSet<DummyPersistentObject>(mockInternalSession, mockRegistry, mockBackingStore, mockQueryFactory, new[] {mockQuery});

            mockRegisteredGraph = MockRepository.GenerateStub<IRegisteredGraph<DummyPersistentObject>>();
            mockRegistry.Stub(_ => _.GetRegistrationFor(Arg<Type>.Is.Anything)).Return(mockRegisteredGraph);

            mockStoredGraph = MockRepository.GenerateStub<IStoredGraph>();
            mockBackingStore.Stub(_ => _.Get(null)).IgnoreArguments().Return(new[] {mockStoredGraph});

            mockTrack = MockRepository.GenerateStub<ITrack<DummyPersistentObject>>();
            mockTrack.Stub(_ => _.Graph).Return(new DummyPersistentObject());
        }

        protected override void When()
        {
            mockInternalSession.Expect(
                _ =>
                _.Track<DummyPersistentObject>(
                    Arg<IStoredGraph>.Is.Same(mockStoredGraph),
                    Arg<IRegisteredGraph>.Is.Same(mockRegisteredGraph)))
                .Return(mockTrack);

            actual = subject.GetEnumerator();
            actual.MoveNext();
        }

        [Then]
        public void it_should_track_the_stored_graph()
        {
            mockInternalSession.VerifyAllExpectations();
        }

        [Then]
        public void it_should_return_an_object_matching_the_query()
        {
            actual.Current.ShouldBeOfType<DummyPersistentObject>();
        }
    }
}