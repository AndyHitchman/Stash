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
    using Engine;
    using Microsoft.WindowsAzure.StorageClient;
    using Serializers;
    using Stash.Azure;
    using Stash.Azure.AzureQueries;
    using Stash.Azure.Specifications.Support;
    using Stash.BackingStore;
    using Stash.Engine;
    using Stash.Queries;

    public class when_executing_less_than : with_int_indexer
    {
        private TrackedGraph equaltrackedGraph;
        private TrackedGraph lessThanTrackedGraph;
        private TrackedGraph greaterThanTrackedGraph;
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            equaltrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, AccessCondition.None, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)}, RegisteredGraph);

            lessThanTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, AccessCondition.None, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 99)}, RegisteredGraph);

            greaterThanTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, AccessCondition.None, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101)}, RegisteredGraph);

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(equaltrackedGraph);
                        _.InsertGraph(lessThanTrackedGraph);
                        _.InsertGraph(greaterThanTrackedGraph);
                    });

            query = new LessThanQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, 100);
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
        public void it_should_get_the_correct_graphs()
        {
            actual.Any(_ => _.InternalId == lessThanTrackedGraph.StoredGraph.InternalId).ShouldBeTrue();
        }
    }
}