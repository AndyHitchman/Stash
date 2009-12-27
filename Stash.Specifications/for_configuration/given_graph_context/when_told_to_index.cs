namespace Stash.Specifications.for_configuration.given_graph_context
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_index : with_dummy_object_context
    {
        [Test]
        public void it_should_throw_nix()
        {
            typeof(NotImplementedException)
                .ShouldBeThrownBy(
                () =>
                Sut.IndexWith(new DummerIndexer()));
        }
    }
}