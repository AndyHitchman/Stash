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
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    [TestFixture]
    public class when_told_to_get_the_internal_id_of_a_graph
    {
        [Test]
        public void it_should_return_the_internal_id_if_the_graph_is_tracked()
        {
            var sut = new StandInInternalSession(null, null);
            var mockPersistentEvent = MockRepository.GenerateStub<IPersistenceEvent>();

            var expected = Guid.NewGuid();
            var graph = new object();
            mockPersistentEvent.Stub(_ => _.UntypedGraph).Return(graph);
            mockPersistentEvent.Stub(_ => _.InternalId).Return(expected);
            
            sut.ExposedPersistenceEvents.Add(mockPersistentEvent);

            sut.InternalIdOfTrackedGraph(graph).ShouldEqual(expected);
        }

        [Test]
        public void it_should_return_null_if_the_graph_is_not_tracked()
        {
            var sut = new InternalSession(null, null);
            
            sut.InternalIdOfTrackedGraph(new object()).ShouldBeNull();
        }
    }
}