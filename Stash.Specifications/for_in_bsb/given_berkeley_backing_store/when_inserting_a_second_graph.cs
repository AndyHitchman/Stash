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
    public class when_inserting_a_second_graph : with_temp_dir
    {
        private ITrackedGraph firstTrackedGraph;
        private ITrackedGraph secondTrackedGraph;
        private readonly Type commonSuperType = typeof(bool);
        private readonly Type firstGraphDistinctSuperType = typeof(float);
        private readonly Type secondGraphDistinctSuperType = typeof(int);
        private readonly Type commonConcreteType = typeof(string);
        private const int FirstIndexDistinctIndexValue = 1;
        private const int CommonIndexValues = 2;
        private const int SecondIndexDistinctIndexValue = 3;
        private const string IndexName = "firstIndex";

        protected override void Given()
        {
            firstTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "thisistheserialisedgraphofthefirstobject".Select(_ => (byte)_),
                commonConcreteType,
                new[] {firstGraphDistinctSuperType, commonSuperType},
                new IProjectedIndex[] {new ProjectedIndex<int>(IndexName, FirstIndexDistinctIndexValue), new ProjectedIndex<int>(IndexName, CommonIndexValues)}
                );

            secondTrackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "thesecondobjectsserialisedgraph".Select(_ => (byte)_),
                commonConcreteType,
                new[] {secondGraphDistinctSuperType, commonSuperType},
                new IProjectedIndex[] {new ProjectedIndex<int>(IndexName, CommonIndexValues), new ProjectedIndex<int>(IndexName, SecondIndexDistinctIndexValue)}
                );

            Subject.EnsureIndex(IndexName, typeof(int));
            Subject.InTransactionDo(_ => _.InsertGraph(firstTrackedGraph));
        }

        protected override void When()
        {
            Subject.InTransactionDo(_ => _.InsertGraph(secondTrackedGraph));
        }

        [Then]
        public void it_should_persist_both_using_the_internal_id_as_the_key()
        {
            Subject.GraphDatabase.ShouldHaveKey(firstTrackedGraph.InternalId);
            Subject.GraphDatabase.ShouldHaveKey(secondTrackedGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_serialised_graph_data_of_both()
        {
            Subject.GraphDatabase.ValueForKey(firstTrackedGraph.InternalId).ShouldEqual(firstTrackedGraph.SerialisedGraph.ToArray());
            Subject.GraphDatabase.ValueForKey(secondTrackedGraph.InternalId).ShouldEqual(secondTrackedGraph.SerialisedGraph.ToArray());
        }

        [Then]
        public void it_should_persist_the_concrete_type_of_both_graphs()
        {
            Subject.ConcreteTypeDatabase.ValueForKey(firstTrackedGraph.InternalId).ShouldEqual(firstTrackedGraph.ConcreteType.FullName.Select(_ => (byte)_));
            Subject.ConcreteTypeDatabase.ValueForKey(secondTrackedGraph.InternalId).ShouldEqual(secondTrackedGraph.ConcreteType.FullName.Select(_ => (byte)_));
        }

        [Then]
        public void it_should_persist_the_concrete_type_in_the_type_hierarchy_of_both_graphs()
        {
            var valuesForKey = Subject.IndexDatabases[BerkeleyBackingStore.TypeHierarchyIndexName].IndexDatabase
                .ValuesForKey(commonConcreteType).Select(_ => _.AsGuid());
            valuesForKey.ShouldContain(firstTrackedGraph.InternalId);
            valuesForKey.ShouldContain(secondTrackedGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_common_super_types_in_the_type_hierarchy_of_both_graphs()
        {
            var valuesForKey = Subject.IndexDatabases[BerkeleyBackingStore.TypeHierarchyIndexName].IndexDatabase
                .ValuesForKey(commonSuperType).Select(_ => _.AsGuid());
            valuesForKey.ShouldContain(firstTrackedGraph.InternalId);
            valuesForKey.ShouldContain(secondTrackedGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_distinct_super_type_of_the_first_graph_in_the_type_hierarchy()
        {
            Subject.IndexDatabases[BerkeleyBackingStore.TypeHierarchyIndexName].IndexDatabase
                .ValuesForKey(firstGraphDistinctSuperType).Select(_ => _.AsGuid())
                .ShouldContain(firstTrackedGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_distinct_super_type_of_the_second_graph_in_the_type_hierarchy()
        {
            Subject.IndexDatabases[BerkeleyBackingStore.TypeHierarchyIndexName].IndexDatabase
                .ValuesForKey(secondGraphDistinctSuperType).Select(_ => _.AsGuid())
                .ShouldContain(secondTrackedGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_common_values_of_the_index_projection()
        {
            var valuesForKey = Subject.IndexDatabases[IndexName].IndexDatabase
                .ValuesForKey(CommonIndexValues.AsByteArray()).Select(_ => _.AsGuid());
            valuesForKey.ShouldContain(firstTrackedGraph.InternalId);
            valuesForKey.ShouldContain(secondTrackedGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_distinct_index_projection_value_of_the_second_graph()
        {
            Subject.IndexDatabases[IndexName].IndexDatabase
                .ValueForKey(SecondIndexDistinctIndexValue.AsByteArray())
                .ShouldEqual(secondTrackedGraph.InternalId.ToByteArray());
        }

        [Then]
        public void it_should_persist_the_distinct_index_projection_value_of_the_first_graph()
        {
            Subject.IndexDatabases[IndexName].IndexDatabase
                .ValueForKey(FirstIndexDistinctIndexValue.AsByteArray())
                .ShouldEqual(firstTrackedGraph.InternalId.ToByteArray());
        }
    }
}