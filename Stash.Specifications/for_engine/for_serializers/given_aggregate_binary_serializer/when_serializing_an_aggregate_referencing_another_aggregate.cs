namespace Stash.Specifications.for_engine.for_serializers.given_aggregate_binary_serializer
{
    using System;
    using Configuration;
    using Engine.Serializers;
    using NUnit.Framework;
    using Support;

    [Ignore("NYI")]
    public class when_serializing_an_aggregate_referencing_another_aggregate : AutoMockedSpecification<AggregateBinarySerializer<Order>>
    {
        private Order actual;
        private IRegisteredGraph<Order> mockRegisteredGraph;

        protected override void Given()
        {
            actual = new Order();
        }

        protected override void When()
        {
            Subject.Serialize(actual, mockRegisteredGraph);
        }
    }
}