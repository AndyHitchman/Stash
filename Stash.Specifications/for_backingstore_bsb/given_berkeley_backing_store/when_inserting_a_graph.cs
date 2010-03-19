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

namespace Stash.Specifications.for_backingstore_bsb.given_berkeley_backing_store
{
    using System;
    using System.Linq;
    using BackingStore;
    using BackingStore.BDB;
    using Configuration;
    using Engine;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_inserting_a_graph : with_temp_dir
    {
        private ITrackedGraph trackedGraph;
        private RegisteredGraph<ClassWithTwoAncestors> registeredGraph;
        private RegisteredIndexer<ClassWithTwoAncestors, int> firstRegisteredIndexer;
        private RegisteredIndexer<ClassWithTwoAncestors, string> secondRegisteredIndexer;
        private IRegistry registry;

        protected override void Given()
        {
            registry = new Registry();
            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>(registry);
            firstRegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex());
            secondRegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, string>(new StringIndex());
            registry.RegisteredIndexers.Add(firstRegisteredIndexer);
            registry.RegisteredIndexers.Add(secondRegisteredIndexer);

            trackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(firstRegisteredIndexer, 1), new ProjectedIndex<string>(secondRegisteredIndexer, "wibble")},
                registeredGraph
                );

            Subject.EnsureIndex(new RegisteredIndexer<Type, string>(new StashTypeHierarchy()));
            Subject.EnsureIndex(firstRegisteredIndexer);
            Subject.EnsureIndex(secondRegisteredIndexer);
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
            Subject.ConcreteTypeDatabase.ValueForKey(trackedGraph.InternalId).ShouldEqual(trackedGraph.GraphType.AssemblyQualifiedName.Select(_ => (byte)_));
        }

        [Then]
        public void it_should_persist_the_type_hierarchy_of_the_graph()
        {
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ValueForKey(trackedGraph.TypeHierarchy.First())
                .ShouldEqual(trackedGraph.InternalId.ToByteArray());
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ValueForKey(trackedGraph.TypeHierarchy.Skip(1).First())
                .ShouldEqual(trackedGraph.InternalId.ToByteArray());
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ValueForKey(trackedGraph.TypeHierarchy.Skip(2).First())
                .ShouldEqual(trackedGraph.InternalId.ToByteArray());
        }

        [Then]
        public void it_should_persist_the_value_of_the_first_index_projection()
        {
            var projectedIndex = trackedGraph.ProjectedIndexes.First();

            Subject.IndexDatabases[firstRegisteredIndexer.IndexName].Index
                .ValueForKey(((int)projectedIndex.UntypedKey).AsByteArray())
                .ShouldEqual(trackedGraph.InternalId.ToByteArray());
        }

        [Then]
        public void it_should_persist_the_value_of_the_second_index_projection()
        {
            var projectedIndex = trackedGraph.ProjectedIndexes.Skip(1).First();

            Subject.IndexDatabases[secondRegisteredIndexer.IndexName].Index
                .ValueForKey(((string)projectedIndex.UntypedKey).AsByteArray())
                .ShouldEqual(trackedGraph.InternalId.ToByteArray());
        }
    }
}