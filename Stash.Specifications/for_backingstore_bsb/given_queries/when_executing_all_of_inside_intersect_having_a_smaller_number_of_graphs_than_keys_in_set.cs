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
    using BackingStore.BDB;
    using BackingStore.BDB.BerkeleyQueries;
    using Engine;
    using Support;

    public class when_executing_all_of_inside_intersect_having_a_smaller_number_of_graphs_than_keys_in_set : with_int_indexer
    {
        private IBerkeleyQuery query;
        private IEnumerable<Guid> actual;
        private TrackedGraph firstMatchingTrackedGraph;
        private TrackedGraph secondMatchingTrackedGraph;
        private TrackedGraph matchingButExcludedByIntersectTrackedGraph;
        private TrackedGraph nonMatchingTrackedGraph;
        private Guid[] joinConstraint;

        protected override void Given()
        {
            firstMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[]
                    {new ProjectedIndex<int>(RegisteredIndexer, 102), new ProjectedIndex<int>(RegisteredIndexer, 101), new ProjectedIndex<int>(RegisteredIndexer, 100)},
                RegisteredGraph
                );

            secondMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[]
                    {
                        new ProjectedIndex<int>(RegisteredIndexer, 102), new ProjectedIndex<int>(RegisteredIndexer, 99), new ProjectedIndex<int>(RegisteredIndexer, 101),
                        new ProjectedIndex<int>(RegisteredIndexer, 100)
                    },
                RegisteredGraph
                );

            matchingButExcludedByIntersectTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 100), new ProjectedIndex<int>(RegisteredIndexer, 101)},
                RegisteredGraph
                );

            nonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 101)},
                RegisteredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(firstMatchingTrackedGraph);
                        _.InsertGraph(secondMatchingTrackedGraph);
                        _.InsertGraph(matchingButExcludedByIntersectTrackedGraph);
                        _.InsertGraph(nonMatchingTrackedGraph);
                    });

            query = new AllOfQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, new[] {102, 100, 101});

            joinConstraint = new[] {firstMatchingTrackedGraph.InternalId, secondMatchingTrackedGraph.InternalId};
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var bsw = (BerkeleyStorageWork)_;
                        return query.ExecuteInsideIntersect(bsw.Transaction, joinConstraint).Materialize();
                    });
        }

        [Then]
        public void it_should_find_two()
        {
            actual.ShouldHaveCount(2);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _ == firstMatchingTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _ == secondMatchingTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}