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
    using Stash.BackingStore;
    using Stash.BerkeleyDB;
    using Stash.BerkeleyDB.BerkeleyQueries;
    using Stash.Engine;
    using Support;

    public class when_executing_is_indexed_inside_intersect : with_int_indexer
    {
        private TrackedGraph firstTrackedGraph;
        private TrackedGraph secondTrackedGraph;
        private TrackedGraph thirdTrackedGraph;
        private TrackedGraph fourthTrackedGraph;
        private IBerkeleyQuery query;
        private IEnumerable<InternalId> actual;
        private InternalId[] joinConstraint;

        protected override void Given()
        {
            firstTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new MemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)},
                RegisteredGraph
                );

            secondTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new MemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)},
                RegisteredGraph
                );

            thirdTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new MemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101)},
                RegisteredGraph
                );

            fourthTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new MemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
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
                        return query.ExecuteInsideIntersect(bsw.Transaction, joinConstraint).Materialize();
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