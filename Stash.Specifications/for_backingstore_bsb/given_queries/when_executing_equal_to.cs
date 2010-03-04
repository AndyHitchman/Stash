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
    using BackingStore.BDB.BerkeleyQueries;
    using Engine;
    using given_berkeley_backing_store;
    using Queries;
    using Support;

    public class when_executing_equal_to : with_int_indexer
    {
        private ITrackedGraph trackedGraph;
        private IQuery query;
        private IEnumerable<IStoredGraph> actual;

        protected override void Given()
        {
            trackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(registeredIndexer, 100)},
                registeredGraph
                );

            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));

            query = new EqualToQuery<int>(registeredIndexer, 100);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(_ => _.Find(registeredGraph, query));
        }

        [Then]
        public void it_should_get_the_only_graph_with_the_matching_index_key()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_get_the_correct_graph()
        {
            actual.First().InternalId.ShouldEqual(trackedGraph.InternalId);
        }
    }
}