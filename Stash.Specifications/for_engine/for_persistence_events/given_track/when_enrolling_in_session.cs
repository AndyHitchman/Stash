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
    using NUnit.Framework;

    [TestFixture]
    public class when_enrolling_in_session
    {
        [Ignore]
        public void it_should_do_nothing_if_the_graph_is_already_tracked()
        {
            //            var mockSession = MockRepository.GenerateMock<IInternalSession>();
            //            var graph = new DummyPersistentObject();
            //            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, Enumerable.Empty<byte>(), mockSession);
            //
            //            mockSession.Expect(s => s.GraphIsTracked(graph)).Return(true);
            //
            //            mockSession.AssertWasNotCalled(s => s.Enroll(null), o => o.IgnoreArguments());
        }

        [Ignore]
        public void it_should_enroll_if_the_graph_is_not_tracked()
        {
            //            var mockSession = MockRepository.GenerateMock<IInternalSession>();
            //            var mockRegistry = MockRepository.GenerateMock<Registry>();
            //            var graph = new DummyPersistentObject();
            //            var sut = new StandInTrack<DummyPersistentObject>(Guid.Empty, graph, mockSession);
            //
            //            mockSession.Expect(s => s.GraphIsTracked(graph)).Return(false);
            //
            //            mockSession.AssertWasCalled(s => s.Enroll(sut));
        }

        [Ignore]
        public void it_should_find_out_if_the_graph_is_already_tracked()
        {
            //            var mockSession = MockRepository.GenerateMock<IInternalSession>();
            //            var graph = new DummyPersistentObject();
            //            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, Enumerable.Empty<byte>(), mockSession);
            //
            //            mockSession.Expect(s => s.GraphIsTracked(graph)).Return(true);
            //
            //            mockSession.VerifyAllExpectations();
        }
    }
}