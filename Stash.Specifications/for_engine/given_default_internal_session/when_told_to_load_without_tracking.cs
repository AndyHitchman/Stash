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

namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using System;
    using System.IO;
    using BackingStore;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Serializers;
    using Support;

    [TestFixture]
    public class when_told_to_load_without_tracking
    {
        public class StandInInternalSession : InternalSession
        {
            public StandInInternalSession(IRegistry registry) : base(registry) {}

            public StandInInternalSession(IRegistry registry, IBackingStore backingStore) : base(registry, backingStore) {}

            public override void Enroll(IPersistenceEvent persistenceEvent)
            {
                Called = true;
            }

            public bool Called { get; set; }
        }

        [Test]
        public void it_should_not_call_enroll()
        {
            var sut = new StandInInternalSession(null, null);

            var mockRegisteredGraph = MockRepository.GenerateStub<IRegisteredGraph<DummyPersistentObject>>();
            var mockStoredGraph = MockRepository.GenerateStub<IStoredGraph>();

            mockStoredGraph.SerialisedGraph = new PreservedMemoryStream(new byte[0]);

            sut.Load(mockStoredGraph, mockRegisteredGraph, null, true);

            sut.Called.ShouldBeFalse();
        }
    }
}