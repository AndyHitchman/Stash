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

namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.Linq;
    using Engine;
    using In.BDB;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_inserting_a_graph : with_temp_dir
    {
        private ITrackedGraph trackedGraph;
        private const string FirstIndexName = "firstIndex";
        private const string SecondIndexName = "secondIndex";

        protected override void Given()
        {
            trackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                typeof(string),
                new[] {typeof(int), typeof(bool)},
                new IProjectedIndex[] {new ProjectedIndex<int>(FirstIndexName, 1), new ProjectedIndex<string>(SecondIndexName, "wibble")}
                );

            Subject.EnsureIndex(FirstIndexName, typeof(int));
            Subject.EnsureIndex(SecondIndexName, typeof(string));
        }

        protected override void When()
        {
            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));
        }

        [Then]
        public void it_should_persist_using_the_internal_id_as_the_key()
        {
            Subject.GraphDatabase.ShouldHaveKey(trackedGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_serialised_graph_data()
        {
            Subject.GraphDatabase.ValueForKey(trackedGraph.InternalId).ShouldEqual(trackedGraph.SerialisedGraph.ToArray());
        }

        [Then]
        public void it_should_persist_the_concrete_type_of_the_graph()
        {
            Subject.ConcreteTypeDatabase.ValueForKey(trackedGraph.InternalId).ShouldEqual(trackedGraph.ConcreteType.FullName.Select(_ => (byte)_));
        }

        [Then]
        public void it_should_persist_the_concrete_type_in_the_type_hierarchy_of_the_graph()
        {
            Subject.TypeHierarchyDatabase.ValueForKey(trackedGraph.ConcreteType).ShouldEqual(trackedGraph.InternalId.ToByteArray());
        }

        [Then]
        public void it_should_persist_the_super_types_in_the_type_hierarchy_of_the_graph()
        {
            Subject.TypeHierarchyDatabase.ValueForKey(trackedGraph.SuperTypes.First()).ShouldEqual(trackedGraph.InternalId.ToByteArray());
            Subject.TypeHierarchyDatabase.ValueForKey(trackedGraph.SuperTypes.Skip(1).First()).ShouldEqual(trackedGraph.InternalId.ToByteArray());
        }

        [Then]
        public void it_should_persist_the_value_of_the_first_index_projection()
        {
            var projectedIndex = trackedGraph.Indexes.First();

            Subject.IndexDatabases[FirstIndexName].IndexDatabase
                .ValueForKey(((int)projectedIndex.UntypedKey).AsByteArray())
                .ShouldEqual(trackedGraph.InternalId.ToByteArray());
        }

        [Then]
        public void it_should_persist_the_value_of_the_second_index_projection()
        {
            var projectedIndex = trackedGraph.Indexes.Skip(1).First();

            Subject.IndexDatabases[SecondIndexName].IndexDatabase
                .ValueForKey(((string)projectedIndex.UntypedKey).AsByteArray())
                .ShouldEqual(trackedGraph.InternalId.ToByteArray());
        }
    }
}