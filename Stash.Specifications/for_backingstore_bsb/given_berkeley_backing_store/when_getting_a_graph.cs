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
    public class when_getting_a_graph : with_temp_dir
    {
        private ITrackedGraph trackedGraph;
        private RegisteredGraph<ClassWithTwoAncestors> registeredGraph;
        private IStoredGraph result;
        private IRegistry registry;

        protected override void Given()
        {
            registry = new Registry();
            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>(registry);

            trackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                Enumerable.Empty<IProjectedIndex>(),
                registeredGraph
                );

            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));
        }

        protected override void When()
        {
            result = Subject.Get(trackedGraph.InternalId);
        }

        [Then]
        public void it_should_return_the_stored_graph_internal_id()
        {
            result.InternalId.ShouldEqual(trackedGraph.InternalId);
        }

        [Then]
        public void it_should_return_the_stored_graph_data()
        {
            result.SerialisedGraph.SequenceEqual(trackedGraph.SerialisedGraph).ShouldBeTrue();
        }
    }
}