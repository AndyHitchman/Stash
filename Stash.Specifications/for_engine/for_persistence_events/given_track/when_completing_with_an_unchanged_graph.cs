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

namespace Stash.Specifications.for_engine.for_persistence_events.given_track
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using Engine.Serializers;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    public class when_completing_with_an_unchanged_graph : AutoMockedSpecification<StandInTrack<DummyPersistentObject>>
    {
        private IStorageWork mockStorageWork;

        protected override void Given()
        {
            Dependency<IStoredGraph>().Expect(_ => _.SerialisedGraph).Return(Enumerable.Repeat<byte>(0x01, 100));
            Dependency<IRegisteredGraph<DummyPersistentObject>>().Expect(_ => _.Serialize(null)).IgnoreArguments().Return(Enumerable.Repeat<byte>(0x01, 100));
            mockStorageWork = MockRepository.GenerateMock<IStorageWork>();
        }

        protected override void When()
        {
            Subject.Complete(mockStorageWork);
        }

        [Then]
        public void it_should_not_waste_time_calculate_indexes()
        {
            Subject.HasCalculatedIndexes.ShouldBeFalse();
        }

        [Then]
        public void it_should_not_tell_storage_work_to_do_an_update()
        {
            mockStorageWork.AssertWasNotCalled(_ => _.UpdateGraph(null), _ => _.IgnoreArguments());
        }
    }
}