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

namespace Stash.Specifications.for_backingstore_bsb.given_queries
{
    using System;
    using System.Linq;
    using BackingStore;
    using BackingStore.BDB;
    using BackingStore.BDB.BerkeleyQueries;
    using Engine;

    public class when_estimating_query_cost : with_int_indexer
    {
        private TrackedGraph equaltrackedGraph;
        private ITrackedGraph lessThanTrackedGraph;
        private TrackedGraph greaterThanTrackedGraph;
        private GreaterThanQuery<int> query;
        private double actual;

        protected override void Given()
        {
            equaltrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 100)},
                RegisteredGraph
                );

            lessThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 99)},
                RegisteredGraph
                );

            greaterThanTrackedGraph = new TrackedGraph(
                new InternalId(Guid.NewGuid()),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer, 101)},
                RegisteredGraph
                );

            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(equaltrackedGraph);
                        _.InsertGraph(lessThanTrackedGraph);
                        _.InsertGraph(greaterThanTrackedGraph);
                    });

            query = new GreaterThanQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, 100);
        }

        protected override void When()
        {
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var berkeleyStorageWork = ((BerkeleyStorageWork)_);
                        return query.EstimatedQueryCost(berkeleyStorageWork.Transaction);
                    });
            Console.WriteLine(actual);
        }
    }
}