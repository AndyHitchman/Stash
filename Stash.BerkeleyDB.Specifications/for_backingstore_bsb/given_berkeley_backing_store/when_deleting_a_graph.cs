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
    public class when_deleting_a_graph : with_temp_dir
    {
        private ITrackedGraph trackedGraph;
        private RegisteredGraph<ClassWithTwoAncestors> registeredGraph;
        private RegisteredIndexer<ClassWithTwoAncestors, int> firstRegisteredIndexer;
        private RegisteredIndexer<ClassWithTwoAncestors, string> secondRegisteredIndexer;

        protected override void Given()
        {
            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>(registry);
            firstRegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex(), registry);
            registry.RegisteredIndexers.Add(firstRegisteredIndexer);
            secondRegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, string>(new StringIndex(), registry);
            registry.RegisteredIndexers.Add(secondRegisteredIndexer);

            trackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), registeredGraph.GraphType, new PreservedMemoryStream("letspretendthisisserialiseddata".Select(_ => (byte)_).ToArray())), new IProjectedIndex[]
                {
                    new ProjectedIndex<int>(firstRegisteredIndexer.IndexName, 1), 
                    new ProjectedIndex<string>(secondRegisteredIndexer.IndexName, "wibble")
                }, registeredGraph);

            Subject.EnsureIndex(firstRegisteredIndexer);
            Subject.EnsureIndex(secondRegisteredIndexer);
            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));
        }

        protected override void When()
        {
            Subject.InTransactionDo(_ => _.DeleteGraph(trackedGraph.StoredGraph.InternalId, registeredGraph));
        }

        [Then]
        public void it_should_have_removed_the_graph()
        {
            Subject.GraphDatabase.ShouldNotHaveKey(trackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_remove_the_concrete_type_of_the_graph()
        {
            Subject.ConcreteTypeDatabase.ShouldNotHaveKey(trackedGraph.StoredGraph.InternalId);
        }

        [Then]
        public void it_should_remove_the_type_hierarchy()
        {
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ShouldNotHaveKey(trackedGraph.TypeHierarchy.ToArray()[0].AsByteArray());
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ShouldNotHaveKey(trackedGraph.TypeHierarchy.ToArray()[1].AsByteArray());
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ShouldNotHaveKey(trackedGraph.TypeHierarchy.ToArray()[2].AsByteArray());
        }

        [Then]
        public void it_should_remove_the_first_index_projection()
        {
            var projectedIndex = trackedGraph.ProjectedIndexes.First();

            Subject.IndexDatabases[firstRegisteredIndexer.IndexName].Index.ShouldNotHaveKey(((int)((ProjectedIndex)projectedIndex).UntypedKey).AsByteArray());
        }

        [Then]
        public void it_should_remove_the_second_index_projection()
        {
            var projectedIndex = trackedGraph.ProjectedIndexes.Skip(1).First();

            Subject.IndexDatabases[secondRegisteredIndexer.IndexName].Index.ShouldNotHaveKey(((string)((ProjectedIndex)projectedIndex).UntypedKey).AsByteArray());
        }
    }
}