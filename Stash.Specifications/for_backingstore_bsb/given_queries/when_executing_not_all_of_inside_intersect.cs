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
    using BackingStore.BDB;
    using BackingStore.BDB.BerkeleyQueries;
    using Engine;
    using given_berkeley_backing_store;
    using Support;

    public class when_executing_not_all_of_inside_intersect : with_int_indexer
    {
        private IBerkeleyQuery query;
        private IEnumerable<Guid> actual;
        private Guid[] joinConstraint;
        private TrackedGraph firstNonMatchingTrackedGraph;
        private TrackedGraph matchingTrackedGraph;
        private TrackedGraph thirdNonMatchingTrackedGraph;
        private TrackedGraph secondNonMatchingTrackedGraph;
        private TrackedGraph secondMatchingTrackedGraph;

        protected override void Given()
        {
            firstNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            matchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[]
                    {new ProjectedIndex<int>(registeredIndexer, 99), new ProjectedIndex<int>(registeredIndexer, 101), new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            secondMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[]
                    {new ProjectedIndex<int>(registeredIndexer, 99), new ProjectedIndex<int>(registeredIndexer, 101), new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            thirdNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 101), new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            secondNonMatchingTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 101)},
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(thirdNonMatchingTrackedGraph);
                        _.InsertGraph(matchingTrackedGraph);
                        _.InsertGraph(firstNonMatchingTrackedGraph);
                        _.InsertGraph(secondNonMatchingTrackedGraph);
                        _.InsertGraph(secondMatchingTrackedGraph);
                    });

            query = new NotAllOfQuery<int>(registeredIndexer, new[] {101, 100});

            joinConstraint = new[]
                {
                    matchingTrackedGraph.InternalId, firstNonMatchingTrackedGraph.InternalId,
                };
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var bsw = (BerkeleyStorageWork)_;
                        return query.ExecuteInsideIntersect(bsw.BackingStore.IndexDatabases[registeredIndexer.IndexName], bsw.Transaction, joinConstraint).ToList();
                    });
        }

        [Then]
        public void it_should_find_one()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _ == matchingTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}