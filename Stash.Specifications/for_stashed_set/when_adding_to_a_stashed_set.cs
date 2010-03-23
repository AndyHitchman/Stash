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

    public class when_adding_to_a_stashed_set : Specification
    {
        private IInternalSession mockInternalSession;
        private IRegisteredGraph<DummyPersistentObject> mockRegisteredGraph;
        private StashedSet<DummyPersistentObject> subject;
        private DummyPersistentObject expected;

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

            expected = new DummyPersistentObject();
        }

        protected override void When()
        {
            subject.Endure(expected);
        }

        [Then]
        public void it_should_endure_the_transient_graph()
        {
            mockInternalSession.AssertWasCalled(
                _ =>
                _.Endure(
                    Arg<object>.Is.Same(expected),
                    Arg<IRegisteredGraph>.Is.Same(mockRegisteredGraph)));
        }
    }
}