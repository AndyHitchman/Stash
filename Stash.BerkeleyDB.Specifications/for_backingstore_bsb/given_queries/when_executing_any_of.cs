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

namespace Stash.BerkeleyDB.Specifications.for_backingstore_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Engine;
    using Serializers;
    using Stash.BackingStore;
    using Stash.BerkeleyDB.BerkeleyQueries;
    using Stash.Engine;
    using Stash.Queries;
    using Support;

    public class when_executing_any_of : with_int_indexer
    {
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;
        private TrackedGraph firstMatchingTrackedGraph;
        private TrackedGraph secondMatchingTrackedGraph;
        private TrackedGraph firstNonMatchingTrackedGraph;
        private TrackedGraph secondtNonMatchingTrackedGraph;

        protected override void Given()
        {
            firstMatchingTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] { new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101) }, RegisteredGraph);

            secondMatchingTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] { new ProjectedIndex<int>(RegisteredIndexer.IndexName, 99) }, RegisteredGraph);

            firstNonMatchingTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] { new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100) }, RegisteredGraph);

            secondtNonMatchingTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] { new ProjectedIndex<int>(RegisteredIndexer.IndexName, 103) }, RegisteredGraph);

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(firstMatchingTrackedGraph);
                        _.InsertGraph(secondMatchingTrackedGraph);
                        _.InsertGraph(firstNonMatchingTrackedGraph);
                        _.InsertGraph(secondtNonMatchingTrackedGraph);
                    });

            query = new AnyOfQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, new[] {101, 99});
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Get(query));
        }

        [Then]
        public void it_should_find_two()
        {
            actual.ShouldHaveCount(2);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _.InternalId == firstMatchingTrackedGraph.StoredGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == secondMatchingTrackedGraph.StoredGraph.InternalId).ShouldBeTrue();
        }
    }
}