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

namespace Stash.Azure.Specifications.given_queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Stash.Azure;
    using Stash.Azure.AzureQueries;
    using Stash.BackingStore;
    using Stash.Engine;
    using Support;

    public class when_executing_starts_with_inside_intersect : with_string_indexer
    {
        private TrackedGraph lessThanTrackedGraph;
        private TrackedGraph lowerTrackedGraph;
        private TrackedGraph upperTrackedGraph;
        private TrackedGraph insideTrackedGraph;
        private TrackedGraph greaterThanTrackedGraph;
        private IAzureQuery query;
        private IEnumerable<InternalId> actual;
        private InternalId[] joinConstraint;

        protected override void Given()
        {
            lessThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<string>(RegisteredIndexer.IndexName, "ml")},
                RegisteredGraph
                );

            lowerTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<string>(RegisteredIndexer.IndexName, "mm")},
                RegisteredGraph
                );

            insideTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<string>(RegisteredIndexer.IndexName, "mma")},
                RegisteredGraph
                );

            upperTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<string>(RegisteredIndexer.IndexName, "mmz")},
                RegisteredGraph
                );

            greaterThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<string>(RegisteredIndexer.IndexName, "mn")},
                RegisteredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(insideTrackedGraph);
                        _.InsertGraph(lowerTrackedGraph);
                        _.InsertGraph(upperTrackedGraph);
                        _.InsertGraph(lessThanTrackedGraph);
                        _.InsertGraph(greaterThanTrackedGraph);
                    });

            query = new StartsWithQuery(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, "mm");

            joinConstraint = new[]
                {
                    insideTrackedGraph.InternalId, greaterThanTrackedGraph.InternalId,
                };
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var bsw = (AzureStorageWork)_;
                        return query.ExecuteInsideIntersect(bsw.ServiceContext, joinConstraint).Materialize();
                    });
        }

        [Then]
        public void it_should_find_one()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_get_the_correct_graphs()
        {
            actual.Any(_ => _ == insideTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}