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
    using Configuration;
    using Engine;
    using In.BDB;
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
            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>();
            firstRegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex());
            registeredGraph.RegisteredIndexers.Add(firstRegisteredIndexer);
            secondRegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, string>(new StringIndex());
            registeredGraph.RegisteredIndexers.Add(secondRegisteredIndexer);

            trackedGraph = new TrackedGraph(
                Guid.NewGuid(),
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                new IProjectedIndex[] {new ProjectedIndex<int>(firstRegisteredIndexer, 1), new ProjectedIndex<string>(secondRegisteredIndexer, "wibble")},
                registeredGraph
                );

            Subject.EnsureIndex(registeredGraph.RegisteredIndexers[0]);
            Subject.EnsureIndex(registeredGraph.RegisteredIndexers[1]);
            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));
        }

        protected override void When()
        {
            Subject.InTransactionDo(_ => _.DeleteGraph(trackedGraph.InternalId, registeredGraph));
        }

        [Then]
        public void it_should_have_removed_the_graph()
        {
            Subject.GraphDatabase.ShouldNotHaveKey(trackedGraph.InternalId);
        }

        [Then]
        public void it_should_remove_the_concrete_type_of_the_graph()
        {
            Subject.ConcreteTypeDatabase.ShouldNotHaveKey(trackedGraph.InternalId);
        }

        [Then]
        public void it_should_remove_the_type_hierarchy()
        {
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ShouldNotHaveKey(trackedGraph.TypeHierarchy.First().AsByteArray());
            Subject.IndexDatabases[Subject.RegisteredTypeHierarchyIndex.IndexName].Index
                .ShouldNotHaveKey(trackedGraph.TypeHierarchy.Skip(1).First().AsByteArray());
        }

        [Then]
        public void it_should_remove_the_first_index_projection()
        {
            var projectedIndex = trackedGraph.ProjectedIndexes.First();

            Subject.IndexDatabases[firstRegisteredIndexer.IndexName].Index.ShouldNotHaveKey(((int)projectedIndex.UntypedKey).AsByteArray());
        }

        [Then]
        public void it_should_remove_the_second_index_projection()
        {
            var projectedIndex = trackedGraph.ProjectedIndexes.Skip(1).First();

            Subject.IndexDatabases[secondRegisteredIndexer.IndexName].Index.ShouldNotHaveKey(((string)projectedIndex.UntypedKey).AsByteArray());
        }
    }
}