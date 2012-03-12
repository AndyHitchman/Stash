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
    using System.Linq;
    using Stash.BackingStore;
    using Stash.BerkeleyDB;
    using Stash.BerkeleyDB.BerkeleyQueries;
    using Stash.Engine;
    using Support;

    public class when_executing_less_than_inside_intersect : with_int_indexer
    {
        private TrackedGraph equalToTrackedGraph;
        private TrackedGraph secondLessThanTrackedGraph;
        private TrackedGraph secondGreaterThanTrackedGraph;
        private TrackedGraph lessThanTrackedGraph;
        private TrackedGraph greaterThanTrackedGraph;
        private IBerkeleyQuery query;
        private IEnumerable<InternalId> actual;
        private InternalId[] joinConstraint;

        protected override void Given()
        {
            lessThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 99)},
                RegisteredGraph
                );

            secondLessThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 100)},
                RegisteredGraph
                );

            equalToTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 101)},
                RegisteredGraph
                );

            greaterThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 102)},
                RegisteredGraph
                );

            secondGreaterThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 103)},
                RegisteredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(equalToTrackedGraph);
                        _.InsertGraph(secondLessThanTrackedGraph);
                        _.InsertGraph(secondGreaterThanTrackedGraph);
                        _.InsertGraph(lessThanTrackedGraph);
                        _.InsertGraph(greaterThanTrackedGraph);
                    });

            query = new LessThanQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, 100);

            joinConstraint = new[]
                {
                    greaterThanTrackedGraph.InternalId, lessThanTrackedGraph.InternalId,
                };
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
        public void it_should_find_one()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.Any(_ => _ == lessThanTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}