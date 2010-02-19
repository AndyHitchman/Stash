namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.IO;
    using System.Linq;
    using Engine;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_inserting_a_graph : with_temp_dir
    {
        private ITrackedGraph trackedGraph;

        protected override void Given()
        {
            trackedGraph = new TrackedGraph(
                Guid.NewGuid(), 
                "letspretendthisisserialiseddata".Select(_ => (byte)_),
                typeof(string),
                new[] { typeof(int), typeof(bool) },
                new IProjectedIndex[] { new ProjectedIndex<int>("firstIndex", 1), new ProjectedIndex<string>("secondIndex", "wibble") }
                );
        }

        protected override void When()
        {
            Subject.InsertGraph(trackedGraph);
        }

        [Then]
        public void it_should_persist_using_the_internal_id_as_the_key()
        {
            Subject.ShouldHaveKeyInPrimary(trackedGraph.InternalId);
        }

        [Then]
        public void it_should_persist_the_serialised_graph_data()
        {
            Subject.ValueForPrimaryKey(trackedGraph.InternalId).ShouldEqual(trackedGraph.SerialisedGraph.ToArray());
        }
    }
}