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

namespace Stash.Specifications.for_backingstore_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using BackingStore.BDB.BerkeleyQueries;
    using Engine;
    using Queries;
    using Support;

    public class when_executing_not_all_of : with_int_indexer
    {
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;
        private TrackedGraph thirdNonMatchingTrackedGraph;
        private TrackedGraph matchingTrackedGraph;
        private TrackedGraph firstNonMatchingTrackedGraph;
        private TrackedGraph secondNonMatchingTrackedGraph;

        protected override void Given()
        {
            firstNonMatchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 100)},
                RegisteredGraph
                );

            matchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[]
                    {new ProjectedIndex<int>(RegisteredIndexer, 99), new ProjectedIndex<int>(RegisteredIndexer, 101), new ProjectedIndex<int>(RegisteredIndexer, 100)},
                RegisteredGraph
                );

            thirdNonMatchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 101), new ProjectedIndex<int>(RegisteredIndexer, 100)},
                RegisteredGraph
                );

            secondNonMatchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 101)},
                RegisteredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(thirdNonMatchingTrackedGraph);
                        _.InsertGraph(matchingTrackedGraph);
                        _.InsertGraph(firstNonMatchingTrackedGraph);
                        _.InsertGraph(secondNonMatchingTrackedGraph);
                    });

            query = new NotAllOfQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, new[] {101, 100});
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Get(query));
        }

        [Then]
        public void it_should_find_one()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _.InternalId == matchingTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}