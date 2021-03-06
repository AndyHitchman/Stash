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

namespace Stash.Specifications.for_engine.for_persistence_events.given_endure
{
    using System.IO;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Engine.PersistenceEvents;
    using Rhino.Mocks;
    using Serializers;
    using Support;

    public class when_completing : Specification
    {
        private IRegisteredGraph<DummyPersistentObject> mockRegisteredGraph;
        private IStorageWork mockStorageWork;
        private IStoredGraph mockStoredGraph;
        private Endure subject;

        protected override void Given()
        {
            mockRegisteredGraph = MockRepository.GenerateMock<IRegisteredGraph<DummyPersistentObject>>();
            mockStorageWork = MockRepository.GenerateMock<IStorageWork>();
            mockStoredGraph = MockRepository.GenerateMock<IStoredGraph>();

            mockRegisteredGraph.Expect(_ => _.Serialize(null, null)).IgnoreArguments().Return(new PreservedMemoryStream());
            mockRegisteredGraph.Expect(_ => _.IndexersOnGraph).Return(Enumerable.Empty<IRegisteredIndexer>());
            mockRegisteredGraph.Expect(_ => _.CreateStoredGraph()).Return(mockStoredGraph);

            subject = new Endure(null, mockRegisteredGraph);
        }

        protected override void When()
        {
            subject.Complete(mockStorageWork, null);
        }


        [Then]
        public void it_should_tell_storage_work_to_insert_the_graph()
        {
            mockStorageWork.AssertWasCalled(_ => _.InsertGraph(Arg<ITrackedGraph>.Is.Anything));
        }
    }
}