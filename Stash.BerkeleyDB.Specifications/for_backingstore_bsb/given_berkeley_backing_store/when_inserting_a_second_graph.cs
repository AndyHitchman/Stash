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

namespace Stash.BerkeleyDB.Specifications.for_backingstore_bsb.given_berkeley_backing_store
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Engine;
    using Serializers;
    using Stash.BackingStore;
    using Stash.Configuration;
    using Stash.Engine;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_inserting_a_second_graph : with_temp_dir
    {
        private RegisteredGraph<ClassWithTwoAncestors> firstRegisteredGraph;
        private RegisteredGraph<OtherClassWithTwoAncestors> secondRegisteredGraph;
        private ITrackedGraph firstTrackedGraph;
        private ITrackedGraph secondTrackedGraph;
        private RegisteredIndexer<ClassWithTwoAncestors, int> registeredIndexer;
        private const int firstIndexDistinctIndexValue = 1;
        private const int commonIndexValues = 2;
        private const int secondIndexDistinctIndexValue = 3;

        protected override void Given()
        {
            firstRegisteredGraph = new RegisteredGraph<ClassWithTwoAncestors>(registry);
            secondRegisteredGraph = new RegisteredGraph<OtherClassWithTwoAncestors>(registry);
            registeredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex(), registry);
            registry.RegisteredIndexers.Add(registeredIndexer);

            firstTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), firstRegisteredGraph.GraphType, new PreservedMemoryStream("thisistheserialisedgraphofthefirstobject".Select(_ => (byte)_).ToArray())), new IProjectedIndex[]
                {
                    new ProjectedIndex<int>(registeredIndexer.IndexName, firstIndexDistinctIndexValue),
                    new ProjectedIndex<int>(registeredIndexer.IndexName, commonIndexValues)
                }, firstRegisteredGraph);

            secondTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), secondRegisteredGraph.GraphType, new PreservedMemoryStream("thesecondobjectsserialisedgraph".Select(_ => (byte)_).ToArray())), new IProjectedIndex[]
                {
                    new ProjectedIndex<int>(registeredIndexer.IndexName, commonIndexValues),
                    new ProjectedIndex<int>(registeredIndexer.IndexName, secondIndexDistinctIndexValue)
                }, secondRegisteredGraph);

            Subject.EnsureIndex(registeredIndexer);
            Subject.InTransactionDo(_ => _.InsertGraph(firstTrackedGraph));
        }

        protected override void When()
        {
            Subject.InTransactionDo(_ => _.InsertGraph(secondTrackedGraph));
        }

        [Then]
        public void it_should_persist_both_using_the_internal_id_as_the_key()
        {
            Subject.GraphDatabase.ShouldHaveKey(firstTrackedGraph.StoredGraph.InternalId);
            Subject.GraphDatabase.ShouldHaveKey(secondTrackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_serialised_graph_data_of_both()
        {
            Subject.GraphDatabase
                .ValueForKey(firstTrackedGraph.StoredGraph.InternalId).AsStream()
                .ShouldEqual(firstTrackedGraph.StoredGraph.SerialisedGraph);
            Subject.GraphDatabase
                .ValueForKey(secondTrackedGraph.StoredGraph.InternalId).AsStream()
                .ShouldEqual(secondTrackedGraph.StoredGraph.SerialisedGraph);
        }

        [Then]
        public void it_should_persist_the_concrete_type_of_both_graphs()
        {
            Subject.ConcreteTypeDatabase
                .ValueForKey(firstTrackedGraph.StoredGraph.InternalId)
                .ShouldEqual(new UnicodeEncoding().GetBytes(firstTrackedGraph.StoredGraph.GraphType.AssemblyQualifiedName));
            Subject.ConcreteTypeDatabase
                .ValueForKey(secondTrackedGraph.StoredGraph.InternalId)
                .ShouldEqual(new UnicodeEncoding().GetBytes(secondTrackedGraph.StoredGraph.GraphType.AssemblyQualifiedName));
        }

        [Then]
        public void it_should_persist_the_common_types_in_the_type_hierarchy_of_both_graphs()
        {
            var valuesForKey = Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ValuesForKey(typeof(ClassWithNoAncestors).AssemblyQualifiedName).Select(_ => _.AsInternalId());
            valuesForKey.ShouldContain(firstTrackedGraph.StoredGraph.InternalId);
            valuesForKey.ShouldContain(secondTrackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_distinct_type_of_the_first_graph_in_the_type_hierarchy()
        {
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ValuesForKey(typeof(ClassWithTwoAncestors).AssemblyQualifiedName).Select(_ => _.AsInternalId())
                .ShouldContain(firstTrackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_distinct_type_of_the_second_graph_in_the_type_hierarchy()
        {
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ValuesForKey(typeof(OtherClassWithTwoAncestors).AssemblyQualifiedName).Select(_ => _.AsInternalId())
                .ShouldContain(secondTrackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_common_values_of_the_index_projection()
        {
            var valuesForKey = Subject.IndexDatabases[registeredIndexer.IndexName].Index
                .ValuesForKey(commonIndexValues.AsByteArray()).Select(_ => _.AsInternalId());
            valuesForKey.ShouldContain(firstTrackedGraph.StoredGraph.InternalId);
            valuesForKey.ShouldContain(secondTrackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_distinct_index_projection_value_of_the_second_graph()
        {
            Subject.IndexDatabases[registeredIndexer.IndexName].Index
                .ValueForKey(secondIndexDistinctIndexValue.AsByteArray())
                .ShouldEqual(secondTrackedGraph.StoredGraph.InternalId.AsByteArray());
        }

        [Then]
        public void it_should_persist_the_distinct_index_projection_value_of_the_first_graph()
        {
            Subject.IndexDatabases[registeredIndexer.IndexName].Index
                .ValueForKey(firstIndexDistinctIndexValue.AsByteArray())
                .ShouldEqual(firstTrackedGraph.StoredGraph.InternalId.AsByteArray());
        }
    }
}