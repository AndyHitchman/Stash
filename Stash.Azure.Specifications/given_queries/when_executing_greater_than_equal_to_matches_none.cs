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
    using Stash.Azure;
    using Stash.Azure.AzureQueries;
    using Stash.Azure.Specifications.Support;
    using Stash.BackingStore;
    using Stash.Engine;
    using Stash.Queries;

    public class when_executing_greater_than_equal_to_matches_none : with_int_indexer
    {
        private ITrackedGraph lessThanTrackedGraph;
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            lessThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                new MemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray()),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 99)},
                RegisteredGraph
                );

            Subject.InTransactionDo(
                _ => _.InsertGraph(lessThanTrackedGraph));

            query = new GreaterThanEqualToQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, 100);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Get(query));
        }

        [Then]
        public void it_should_find_none()
        {
            actual.ShouldHaveCount(0);
        }
    }
}