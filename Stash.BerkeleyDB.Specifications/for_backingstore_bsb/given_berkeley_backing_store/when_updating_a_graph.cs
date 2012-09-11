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
    public class when_updating_a_graph : with_temp_dir
    {
        private RegisteredGraph<ClassWithTwoAncestors> registeredGraph;
        private ITrackedGraph originalTrackedGraph;
        private TrackedGraph updatedTrackedGraph;
        private RegisteredIndexer<ClassWithTwoAncestors, int> registeredIndexer;

        protected override void Given()
        {
            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>(registry);
            registeredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex(), registry);
            registry.RegisteredIndexers.Add(registeredIndexer);

            originalTrackedGraph = new TrackedGraph(new StoredGraph(new InternalId(Guid.NewGuid()), registeredGraph.GraphType, new PreservedMemoryStream("thisistheserialisedgraphofthefirstobject".Select(_ => (byte)_).ToArray())), new IProjectedIndex[]
                {
                    new ProjectedIndex<int>(registeredIndexer.IndexName, 1),
                    new ProjectedIndex<int>(registeredIndexer.IndexName, 2)
                }, registeredGraph);

            updatedTrackedGraph = new TrackedGraph(new StoredGraph(originalTrackedGraph.StoredGraph.InternalId, registeredGraph.GraphType, new PreservedMemoryStream("thesecondobjectsserialisedgraph".Select(_ => (byte)_).ToArray())), new IProjectedIndex[]
                {
                    new ProjectedIndex<int>(registeredIndexer.IndexName, 2),
                    new ProjectedIndex<int>(registeredIndexer.IndexName, 3)
                }, registeredGraph);

            Subject.EnsureIndex(registeredIndexer);
            Subject.InTransactionDo(_ => _.InsertGraph(originalTrackedGraph));
        }

        protected override void When()
        {
            Subject.InTransactionDo(_ => _.UpdateGraph(updatedTrackedGraph));
        }

        [Then]
        public void it_should_update_the_serialised_graph_data()
        {
            Subject.GraphDatabase.ValueForKey(originalTrackedGraph.StoredGraph.InternalId).AsStream().ShouldEqual(updatedTrackedGraph.StoredGraph.SerialisedGraph);
        }

        [Then]
        public void it_should_remove_the_redundant_index_projection()
        {
            Subject.IndexDatabases[registeredIndexer.IndexName].Index.ShouldNotHaveKey(1.AsByteArray());
        }

        [Then]
        public void it_should_add_the_new_index_projection()
        {
            Subject.IndexDatabases[registeredIndexer.IndexName].Index.ShouldHaveKey(3.AsByteArray());
        }

        [Then]
        public void it_should_retain_the_common_index_projection()
        {
            Subject.IndexDatabases[registeredIndexer.IndexName].Index.ShouldHaveKey(2.AsByteArray());
        }
    }
}