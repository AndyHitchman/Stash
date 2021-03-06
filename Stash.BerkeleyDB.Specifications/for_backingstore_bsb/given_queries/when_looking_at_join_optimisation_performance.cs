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
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Engine;
    using Serializers;
    using Stash.BackingStore;
    using Stash.BerkeleyDB;
    using Stash.BerkeleyDB.BerkeleyQueries;
    using Stash.Engine;
    using Support;

    public class when_looking_at_join_optimisation_performance : with_int_indexer
    {
        private TrackedGraph insideTrackedGraph;
        private TrackedGraph lowerTrackedGraph;
        private TrackedGraph upperTrackedGraph;
        private TrackedGraph lessThanTrackedGraph;
        private TrackedGraph greaterThanTrackedGraph;
        private IBerkeleyQuery query;
        private IEnumerable<InternalId> actual;

        protected override void Given()
        {
            insideTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 101)}, RegisteredGraph);

            lowerTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 100)}, RegisteredGraph);

            upperTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 103)}, RegisteredGraph);

            lessThanTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 99)}, RegisteredGraph);

            greaterThanTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 104)}, RegisteredGraph);

            Subject.InTransactionDo(
                _ =>
                    {
                        var bbs = ((BerkeleyStorageWork)_).BackingStore;
                        _.InsertGraph(insideTrackedGraph);
                        _.InsertGraph(lowerTrackedGraph);
                        _.InsertGraph(upperTrackedGraph);
                        _.InsertGraph(lessThanTrackedGraph);
                        _.InsertGraph(greaterThanTrackedGraph);

                        //Generate some records
                        for(var i = 0; i < 2000; i++)
                        {
                            _.InsertGraph(
                                new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), RegisteredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(b => (byte)b).ToArray())), new IProjectedIndex[] {new ProjectedIndex<int>(RegisteredIndexer.IndexName, 102)}, RegisteredGraph));
                        }
                    });

            query = new BetweenQuery<int>(Subject.IndexDatabases[RegisteredIndexer.IndexName], RegisteredIndexer, 100, 103);
        }

        protected override void When() {}

        [Then]
        public void it_run_faster_with_a_small_number_of_pre_joins()
        {
            Console.WriteLine("Testing");
            var smallJoinConstraint = new[]
                {
                    insideTrackedGraph.StoredGraph.InternalId, greaterThanTrackedGraph.StoredGraph.InternalId
                }
                .Union(Enumerable.Range(1, 50).Select(_ => new InternalId(Guid.NewGuid())))
                .Materialize();
            var largeJoinConstraint = new[]
                {
                    insideTrackedGraph.StoredGraph.InternalId, greaterThanTrackedGraph.StoredGraph.InternalId,
                }
                .Union(Enumerable.Range(1, 5000).Select(_ => new InternalId(Guid.NewGuid())))
                .Materialize();

            //Run once and ignore
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var bsw = (BerkeleyStorageWork)_;
                        return query.ExecuteInsideIntersect(bsw.Transaction, largeJoinConstraint).Materialize();
                    });

            //Run once and ignore
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var bsw = (BerkeleyStorageWork)_;
                        return query.ExecuteInsideIntersect(bsw.Transaction, smallJoinConstraint).Materialize();
                    });

            var nonOptTime = new Stopwatch();
            nonOptTime.Start();
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var bsw = (BerkeleyStorageWork)_;
                        return query.ExecuteInsideIntersect(bsw.Transaction, largeJoinConstraint).Materialize();
                    });
            nonOptTime.Stop();

            var optTime = new Stopwatch();
            optTime.Start();
            actual = Subject.InTransactionDo(
                _ =>
                    {
                        var bsw = (BerkeleyStorageWork)_;
                        return query.ExecuteInsideIntersect(bsw.Transaction, smallJoinConstraint).Materialize();
                    });
            optTime.Stop();

            Console.WriteLine("Non-opt: " + nonOptTime.ElapsedMilliseconds);
            Console.WriteLine("Opt: " + optTime.ElapsedMilliseconds);

            (optTime.ElapsedMilliseconds < nonOptTime.ElapsedMilliseconds).ShouldBeTrue();
        }
    }
}