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

namespace Stash.Specifications.for_backingstore_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using BackingStore.BDB.BerkeleyQueries;
    using Engine;
    using given_berkeley_backing_store;
    using Queries;
    using Support;

    public class when_executing_any_of_having_query_key_that_doesnt_exist : with_int_indexer
    {
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;
        private TrackedGraph firstMatchingTrackedGraph;
        private TrackedGraph secondMatchingTrackedGraph;
        private TrackedGraph firstNonMatchingTrackedGraph;
        private TrackedGraph secondtNonMatchingTrackedGraph;

        protected override void Given()
        {
            firstMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 101)},
                registeredGraph
                );

            secondMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 99)},
                registeredGraph
                );

            firstNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            secondtNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 103)},
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(firstMatchingTrackedGraph);
                        _.InsertGraph(secondMatchingTrackedGraph);
                        _.InsertGraph(firstNonMatchingTrackedGraph);
                        _.InsertGraph(secondtNonMatchingTrackedGraph);
                    });

            query = new AnyOfQuery<int>(registeredIndexer, new[] {101, 98, 99});
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(query));
        }

        [Then]
        public void it_should_find_two()
        {
            actual.ShouldHaveCount(2);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _.InternalId == firstMatchingTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _.InternalId == secondMatchingTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}