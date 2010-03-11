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
    using Queries;
    using Support;

    public class when_executing_is_indexed_inside_intersect : with_int_indexer
    {
        private TrackedGraph firstTrackedGraph;
        private TrackedGraph secondTrackedGraph;
        private TrackedGraph thirdTrackedGraph;
        private TrackedGraph fourthTrackedGraph;
        private IBerkeleyQuery query;
        private List<Guid> actual;
        private Guid[] joinConstraint;

        protected override void Given()
        {
            firstTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            secondTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            thirdTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 101)},
                registeredGraph
                );

            fourthTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 102)},
                registeredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(firstTrackedGraph);
                        _.InsertGraph(secondTrackedGraph);
                        _.InsertGraph(thirdTrackedGraph);
                        _.InsertGraph(fourthTrackedGraph);
                    });

            query = new IsIndexedQuery(registeredIndexer);

            joinConstraint = new[]
                {
                    firstTrackedGraph.InternalId, secondTrackedGraph.InternalId,
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
        public void it_should_find_two()
        {
            actual.ShouldHaveCount(2);
        }

        [Then]
        public void it_should_get_the_correct_graphs()
        {
            actual.Any(_ => _ == firstTrackedGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _ == secondTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}