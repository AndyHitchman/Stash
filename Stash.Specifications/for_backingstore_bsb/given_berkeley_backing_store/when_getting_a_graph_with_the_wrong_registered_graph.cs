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

namespace Stash.Specifications.for_backingstore_bsb.given_berkeley_backing_store
{
    using System;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Engine;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_getting_a_graph_with_the_wrong_registered_graph : with_temp_dir
    {
        private ITrackedGraph trackedGraph;
        private RegisteredGraph<ClassWithTwoAncestors> insertingRegisteredGraph;
        private RegisteredGraph<OtherClassWithTwoAncestors> gettingRegisteredGraph;
        private IStoredGraph result;

        protected override void Given()
        {
            insertingRegisteredGraph = new RegisteredGraph<ClassWithTwoAncestors>();
            gettingRegisteredGraph = new RegisteredGraph<OtherClassWithTwoAncestors>();

            trackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                Enumerable.Empty<IProjectedIndex>(),
                insertingRegisteredGraph
                );

            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));
        }

        protected override void When() {}

        [Then]
        public void it_should_throw_wrong_registered_graph()
        {
            typeof(AttemptToGetWithWrongRegisteredGraphException).ShouldBeThrownBy(
                () => Subject.Get(trackedGraph.InternalId, gettingRegisteredGraph)
                );
        }
    }
}