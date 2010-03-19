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
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Rhino.Mocks;
    using Support;

    public class when_completing : AutoMockedSpecification<StandInTrack<DummyPersistentObject>>
    {
        private IStorageWork mockStorageWork;

        protected override void Given()
        {
            Dependency<IStoredGraph>().Expect(_ => _.SerialisedGraph).Return(Enumerable.Empty<byte>());
            Dependency<IRegisteredGraph<DummyPersistentObject>>().Expect(_ => _.Serialize(null)).IgnoreArguments().Return(Enumerable.Empty<byte>());
            mockStorageWork = MockRepository.GenerateMock<IStorageWork>();
        }

        protected override void When()
        {
            Subject.Complete(mockStorageWork);
        }


        [Then]
        public void it_should_calculate_a_new_hash()
        {
            Subject.CompletionHash.ShouldNotBeNull();
        }
    }
}