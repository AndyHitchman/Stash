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

namespace Stash.BerkeleyDB.Specifications.for_backingstore_bsb.given_berkeley_backing_store
{
    using System;
    using System.IO;
    using System.Linq;
    using Stash.BackingStore;
    using Stash.Configuration;
    using Stash.Engine;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_getting_a_graph : with_temp_dir
    {
        private ITrackedGraph trackedGraph;
        private RegisteredGraph<ClassWithTwoAncestors> registeredGraph;
        private IStoredGraph actual;

        protected override void Given()
        {
            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>(registry);

            trackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new MemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                Enumerable.Empty<IProjectedIndex>(),
                registeredGraph
                );

            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));
        }

        protected override void When()
        {
            actual = Subject.Get(trackedGraph.InternalId);
        }

        [Then]
        public void it_should_return_the_stored_graph_internal_id()
        {
            actual.InternalId.ShouldEqual(trackedGraph.InternalId);
        }

        [Then]
        public void it_should_return_the_stored_graph_data()
        {
//            Console.WriteLine("actual: " + new StreamReader(result.SerialisedGraph).ReadToEnd());
//            Console.WriteLine("expected: " + new StreamReader(trackedGraph.SerialisedGraph).ReadToEnd());
            actual.SerialisedGraph.ShouldEqual(trackedGraph.SerialisedGraph).ShouldBeTrue();
        }
    }
}