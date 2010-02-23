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
    public class when_deleting_a_graph : with_temp_dir
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
            Subject.InTransactionDo(_ => _.InsertGraph(trackedGraph));
        }

        protected override void When()
        {
            Subject.InTransactionDo(_ => _.DeleteGraph(trackedGraph.InternalId));
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
            Subject.IndexDatabases[BerkeleyBackingStore.TypeHierarchyIndexName].IndexDatabase
                .ShouldNotHaveKey(trackedGraph.ConcreteType.AsByteArray());
            Subject.IndexDatabases[BerkeleyBackingStore.TypeHierarchyIndexName].IndexDatabase
                .ShouldNotHaveKey(trackedGraph.SuperTypes.First().AsByteArray());
            Subject.IndexDatabases[BerkeleyBackingStore.TypeHierarchyIndexName].IndexDatabase
                .ShouldNotHaveKey(trackedGraph.SuperTypes.Skip(1).First().AsByteArray());
        }

        [Then]
        public void it_should_remove_the_first_index_projection()
        {
            var projectedIndex = trackedGraph.Indexes.First();

            Subject.IndexDatabases[FirstIndexName].IndexDatabase.ShouldNotHaveKey(((int)projectedIndex.UntypedKey).AsByteArray());
        }

        [Then]
        public void it_should_remove_the_second_index_projection()
        {
            var projectedIndex = trackedGraph.Indexes.Skip(1).First();

            Subject.IndexDatabases[SecondIndexName].IndexDatabase.ShouldNotHaveKey(((string)projectedIndex.UntypedKey).AsByteArray());
        }
    }
}