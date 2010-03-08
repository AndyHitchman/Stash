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

namespace Stash.Specifications.for_configuration.given_registered_stash
{
    using BackingStore;
    using Configuration;
    using Engine;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    [TestFixture]
    public class when_engaging_backing_store
    {
        [Test]
        public void it_should_tell_registered_graphs_to_engage_themselves()
        {
            var mockRegisteredGraph = MockRepository.GenerateMock<RegisteredGraph<DummyPersistentObject>>();
            var fakeBackingStore = MockRepository.GenerateStub<IBackingStore>();
            var sut = new Registry();
            sut.RegisteredGraphs.Add(typeof(DummyPersistentObject), mockRegisteredGraph);

            sut.EngageBackingStore(fakeBackingStore);

            mockRegisteredGraph.AssertWasCalled(graph => graph.EngageBackingStore(fakeBackingStore));
        }
    }
}