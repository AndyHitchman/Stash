namespace Stash.Specifications.for_configuration.given_map_context
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class when_told_to_reduce : with_dummy_map_context
    {
        [Test]
        public void it_should_throw_nix()
        {
            typeof(NotImplementedException)
                .ShouldBeThrownBy(
                () =>
                Sut.ReduceWith(new DummyReducer()));
        }
    }
}