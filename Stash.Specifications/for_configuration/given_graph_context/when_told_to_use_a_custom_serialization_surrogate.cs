namespace Stash.Specifications.for_configuration.given_graph_context
{
    using System;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_told_to_use_a_custom_serialization_surrogate : with_dummy_graph_context
    {
        [Test]
        public void it_should_throw_nix()
        {
            typeof(NotImplementedException)
                .ShouldBeThrownBy(
                () =>
                Sut.SerializeWith(new DummerSerializationSurrogate()));
        }
    }
}