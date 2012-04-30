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
    using Stash.BerkeleyDB;
    using Stash.BerkeleyDB.BerkeleyQueries;
    using Stash.Engine;
    using Support;

    public class when_executing_not_any_of_inside_intersect : with_int_indexer
    {
        private IBerkeleyQuery query;
        private IEnumerable<InternalId> actual;
        private InternalId[] joinConstraint;
        private TrackedGraph firstNonMatchingTrackedGraph;
        private TrackedGraph matchingTrackedGraph;
        private TrackedGraph thirdNonMatchingTrackedGraph;
        private TrackedGraph secondNonMatchingTrackedGraph;
        private TrackedGraph secondMatchingTrackedGraph;

        protected override void Given()
        {
            firstNonMatchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)},
                RegisteredGraph
                );

            secondNonMatchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[]
                    {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 99), new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101), new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)},
                RegisteredGraph
                );

            thirdNonMatchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101), new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)},
                RegisteredGraph
                );

            matchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101)},
                RegisteredGraph
                );

            secondMatchingTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 103)},
                RegisteredGraph
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

            query = new NotAnyOfQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, new[] {99, 100});

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
            actual.Any(_ => _ == matchingTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}