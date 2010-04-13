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

namespace Stash.Specifications.for_engine.for_persistence_events.given_destroy
{
    using System;
    using BackingStore;
    using Configuration;
    using Engine.PersistenceEvents;
    using Rhino.Mocks;
    using Support;

    public class when_completing : Specification
    {
        private IRegisteredGraph<DummyPersistentObject> mockRegisteredGraph;
        private IStorageWork mockStorageWork;
        private Destroy subject;

        protected override void Given()
        {
            mockRegisteredGraph = MockRepository.GenerateMock<IRegisteredGraph<DummyPersistentObject>>();
            mockStorageWork = MockRepository.GenerateMock<IStorageWork>();
            subject = new Destroy(Guid.NewGuid(), null, mockRegisteredGraph);
        }

        protected override void When()
        {
            subject.Complete(mockStorageWork);
        }


        [Then]
        public void it_should_tell_storage_work_to_delete_the_graph_by_id()
        {
            mockStorageWork.AssertWasCalled(_ => _.DeleteGraph(subject.InternalId, mockRegisteredGraph));
        }
    }
}