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
    using Engine;
    using Serializers;
    using Stash.BackingStore;
    using Stash.Configuration;
    using Stash.Engine;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_updating_a_second_graph : with_temp_dir
    {
        private ITrackedGraph firstTrackedGraph;
        private ITrackedGraph secondTrackedGraph;
        private RegisteredGraph<ClassWithTwoAncestors> registeredGraph;
        private RegisteredIndexer<ClassWithTwoAncestors, int> firstRegisteredIndexer;
        private RegisteredIndexer<ClassWithTwoAncestors, string> secondRegisteredIndexer;
        private TrackedGraph updatedSecondTrackedGraph;

        protected override void Given()
        {
            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>(registry);
            firstRegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex(), registry);
            registry.RegisteredIndexers.Add(firstRegisteredIndexer);
            secondRegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, string>(new StringIndex(), registry);
            registry.RegisteredIndexers.Add(secondRegisteredIndexer);

            firstTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), registeredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[]
                {
                    new ProjectedIndex<int>(firstRegisteredIndexer.IndexName, 1), 
                    new ProjectedIndex<string>(secondRegisteredIndexer.IndexName, "wibble")
                }, registeredGraph);

            secondTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), registeredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[]
                {
                    new ProjectedIndex<int>(firstRegisteredIndexer.IndexName, 1), 
                    new ProjectedIndex<string>(secondRegisteredIndexer.IndexName, "wiggle")
                }, registeredGraph);

            updatedSecondTrackedGraph = new TrackedGraph(new StoredGraph(secondTrackedGraph.StoredGraph.InternalId, registeredGraph.GraphType, new PreservedMemoryStream("updateddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[]
                {
                    new ProjectedIndex<int>(firstRegisteredIndexer.IndexName, 2), 
                    new ProjectedIndex<string>(secondRegisteredIndexer.IndexName, "floop")
                }, registeredGraph);

            Subject.EnsureIndex(firstRegisteredIndexer);
            Subject.EnsureIndex(secondRegisteredIndexer);
            Subject.InTransactionDo(
                _ =>
                    {
                        _.InsertGraph(firstTrackedGraph);
                        _.InsertGraph(secondTrackedGraph);
                    });
        }

        protected override void When()
        {
            Subject.InTransactionDo(_ => _.UpdateGraph(updatedSecondTrackedGraph));
        }

        [Then]
        public void it_should_not_have_removed_the_first_graph()
        {
            Subject.GraphDatabase.ShouldHaveKey(firstTrackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_not_have_removed_the_concrete_type_of_the_first_graph()
        {
            Subject.ConcreteTypeDatabase.ShouldHaveKey(firstTrackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_not_have_remove_the_type_hierarchy_of_the_first_graph()
        {
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ShouldHaveKey(firstTrackedGraph.TypeHierarchy.First().AsByteArray());
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ShouldHaveKey(firstTrackedGraph.TypeHierarchy.Skip(1).First().AsByteArray());
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ShouldHaveKey(firstTrackedGraph.TypeHierarchy.Skip(2).First().AsByteArray());
        }

        [Then]
        public void it_should_not_remove_the_first_index_projection_of_the_first_graph()
        {
            var projectedIndex = firstTrackedGraph.ProjectedIndexes.First();

            Subject.IndexDatabases[firstRegisteredIndexer.IndexName].Index.ShouldHaveKey(((int)((ProjectedIndex)projectedIndex).UntypedKey).AsByteArray());
        }

        [Then]
        public void it_should_remove_the_second_index_projection_of_the_first_graph()
        {
            var projectedIndex = firstTrackedGraph.ProjectedIndexes.Skip(1).First();

            Subject.IndexDatabases[secondRegisteredIndexer.IndexName].Index.ShouldHaveKey(((string)((ProjectedIndex)projectedIndex).UntypedKey).AsByteArray());
        }
    }
}