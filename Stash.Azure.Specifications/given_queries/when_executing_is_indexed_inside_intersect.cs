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
    using System.IO;
    using System.Linq;
    using AzureQueries;
    using Engine;
    using Microsoft.WindowsAzure.StorageClient;
    using Serializers;
    using Stash.Azure;
    using Stash.BackingStore;
    using Stash.Engine;
    using Support;

    public class when_executing_is_indexed_inside_intersect : with_int_indexer
    {
        private TrackedGraph firstTrackedGraph;
        private TrackedGraph secondTrackedGraph;
        private TrackedGraph thirdTrackedGraph;
        private TrackedGraph fourthTrackedGraph;
        private IAzureQuery query;
        private IEnumerable<InternalId> actual;
        private InternalId[] joinConstraint;

        protected override void Given()
        {
            firstTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, AccessCondition.None, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)}, RegisteredGraph);

            secondTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, AccessCondition.None, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)}, RegisteredGraph);

            thirdTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, AccessCondition.None, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101)}, RegisteredGraph);

            fourthTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, AccessCondition.None, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 102)}, RegisteredGraph);

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
                    firstTrackedGraph.StoredGraph.InternalId, secondTrackedGraph.StoredGraph.InternalId,
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
        public void it_should_find_two()
        {
            actual.ShouldHaveCount(2);
        }

        [Then]
        public void it_should_get_the_correct_graphs()
        {
            actual.Any(_ => _ == firstTrackedGraph.StoredGraph.InternalId).ShouldBeTrue();
            actual.Any(_ => _ == secondTrackedGraph.StoredGraph.InternalId).ShouldBeTrue();
        }
    }
}