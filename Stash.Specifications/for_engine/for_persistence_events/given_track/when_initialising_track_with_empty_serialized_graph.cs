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

namespace Stash.Specifications.for_engine.for_persistence_events.given_track
{
    using System.IO;
    using System.Linq;
    using BackingStore;
    using Rhino.Mocks;
    using Serializers;
    using Support;

    public class when_initialising_track_with_empty_serialized_graph : AutoMockedSpecification<StandInTrack<DummyPersistentObject>>
    {
        protected override void Given()
        {
            var serializedGraph = new PreservedMemoryStream(Enumerable.Empty<byte>().ToArray());
            Dependency<IStoredGraph>().Expect(_ => _.SerialisedGraph).Return(serializedGraph);
        }

        protected override void When()
        {
            var instantiate = Subject;
        }

        [Then]
        public void it_should_calculate_a_hash_code_based_on_the_empty_serialized_graph()
        {
            Subject.OriginalHash.ShouldNotBeEmpty();
        }
    }
}