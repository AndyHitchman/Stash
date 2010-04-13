namespace Stash.Specifications.for_engine.for_serializers.given_aggregate_binary_serializer
{
    using System;

    [Serializable]
    public class Order
    {
        public string Number { get; set; }
        public Customer ForCustomer { get; set; }
    }
}