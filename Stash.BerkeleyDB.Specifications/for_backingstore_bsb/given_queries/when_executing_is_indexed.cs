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

    public class when_executing_is_indexed : with_int_indexer
    {
        private TrackedGraph firstTrackedGraph;
        private TrackedGraph secondTrackedGraph;
        private TrackedGraph thirdTrackedGraph;
        private TrackedGraph fourthTrackedGraph;
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            firstTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)},
                RegisteredGraph
                );

            secondTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)},
                RegisteredGraph
                );

            thirdTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101)},
                RegisteredGraph
                );

            fourthTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 102)},
                RegisteredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(firstTrackedGraph);
                        _.InsertGraph(secondTrackedGraph);
                        _.InsertGraph(thirdTrackedGraph);
                        _.InsertGraph(fourthTrackedGraph);
                    });

            query = new IsIndexedQuery(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Get(query));
        }

        [Then]
        public void it_should_find_four()
        {
            actual.ShouldHaveCount(4);
        }

        [Then]
        public void it_should_get_the_correct_graphs()
        {
            actual.Any(_ => _.InternalId == firstTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == secondTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == thirdTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == fourthTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}