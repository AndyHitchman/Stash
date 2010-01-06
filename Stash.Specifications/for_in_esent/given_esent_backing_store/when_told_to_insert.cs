namespace Stash.Specifications.for_in_esent.given_esent_backing_store
{
    using System;
    using System.IO;
    using Engine;
    using NUnit.Framework;

    [TestFixture][Ignore]
    public class when_told_to_insert : with_a_dummy_database
    {
        [Test]
        public void it_should_complain_if_the_persistent_object_set_is_null()
        {
            typeof(ArgumentNullException).ShouldBeThrownBy(
                () =>
                Sut.InsertGraphs(null)
                );
        }
        [Test]
        public void it_should_complain_if_the_persistent_object_is_null()
        {
            PersistentGraph persistentGraph = null;
            typeof(ArgumentNullException).ShouldBeThrownBy(
                () =>
                Sut.InsertGraphs(new[] {persistentGraph})
                );
        }

        [Test]
        public void it_should_complain_if_the_func_to_get_the_serialization_stream_is_null()
        {
            var persistentGraph = new PersistentGraph(new[] { typeof(DummyPersistentObject) }, null);
            typeof(ArgumentNullException).ShouldBeThrownBy(
                () =>
                Sut.InsertGraphs(new[] { persistentGraph })
                );
        }

        [Test]
        public void it_should_complain_if_the_func_to_get_the_serialization_stream_returns_null()
        {
            var persistentGraph = new PersistentGraph(new[] { typeof(DummyPersistentObject) }, () => null);
            typeof(ArgumentNullException).ShouldBeThrownBy(
                () =>
                Sut.InsertGraphs(new[] { persistentGraph })
                );
        }
    }
}