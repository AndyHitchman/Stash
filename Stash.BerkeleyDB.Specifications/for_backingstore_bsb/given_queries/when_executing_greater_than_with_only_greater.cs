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
    using Stash.BerkeleyDB.BerkeleyQueries;
    using Stash.Engine;
    using Stash.Queries;
    using Support;

    public class when_executing_greater_than_with_only_greater : with_int_indexer
    {
        private TrackedGraph greaterThanTrackedGraph;
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            greaterThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 101)},
                RegisteredGraph
                );

            Subject.InTransactionDo(
                _ => _.InsertGraph(greaterThanTrackedGraph));

            query = new GreaterThanQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, 100);
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
            actual.Any(_ => _.InternalId == greaterThanTrackedGraph.InternalId).ShouldBeTrue();
        }
    }
}