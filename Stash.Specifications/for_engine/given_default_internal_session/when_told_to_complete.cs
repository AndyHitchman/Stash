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

namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using System;
    using BackingStore;
    using Configuration;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;
    using Support;

    [TestFixture]
    public class when_told_to_complete : AutoMockedSpecification<StandInInternalSession>
    {
        private IPersistenceEvent mockPersistentEvent;
        private IBackingStore mockBackingStore;

        protected override void Given()
        {
            mockPersistentEvent = MockRepository.GenerateStub<IPersistenceEvent>();
            Subject.ExposedPersistenceEvents.Add(mockPersistentEvent);

            //Call the supplied delegate.
            var mockStorageWork = MockRepository.GenerateStub<IStorageWork>();
            Dependency<IBackingStore>().Stub(_ => _.InTransactionDo(null))
                .IgnoreArguments()
                .WhenCalled(_ => ((Action<IStorageWork>)_.Arguments[0]).Invoke(mockStorageWork));
        }

        protected override void When()
        {
            Subject.Complete();
        }

        [Then]
        public void it_should_clear_all_peristed_events()
        {
            Subject.EnrolledPersistenceEvents.ShouldBeEmpty();
        }

        [Then]
        public void it_should_tell_persisted_events_to_complete()
        {
            mockPersistentEvent.AssertWasCalled(_ => _.Complete(null), _ => _.IgnoreArguments());
        }
    }
}