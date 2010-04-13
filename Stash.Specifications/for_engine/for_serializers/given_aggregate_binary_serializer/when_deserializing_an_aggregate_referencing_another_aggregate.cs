namespace Stash.Specifications.for_engine.for_serializers.given_aggregate_binary_serializer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Configuration;
    using Engine;
    using Engine.Serializers;
    using Engine.Serializers.Binary;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Support;

    public class when_deserializing_an_aggregate_referencing_another_aggregate : AutoMockedSpecification<AggregateBinarySerializer<Order>>
    {
        private Order root;
        private Guid internalIdOfCustomer;
        private IInternalSession mockSession;
        private Order actual;
        private IRegistry mockRegistry;
        private Customer customer;

        protected override void Given()
        {
            customer = new Customer { Name = "Bob" };
            root = new Order { ForCustomer = customer };

            internalIdOfCustomer = Guid.NewGuid();

            mockRegistry = MockRepository.GenerateMock<IRegistry>();
            mockRegistry.Stub(_ => _.IsManagingGraphTypeOrAncestor(typeof(Customer))).Return(true);

            mockSession = MockRepository.GenerateMock<IInternalSession>();
            mockSession.Stub(_ => _.InternalIdOfTrackedGraph(root.ForCustomer)).Return(internalIdOfCustomer);

            Dependency<IRegisteredGraph<Order>>().Stub(_ => _.GraphType).Return(typeof(Order));
            Dependency<IRegisteredGraph<Order>>().Stub(_ => _.Registry).Return(mockRegistry);
        }

        protected override void When()
        {
            mockSession.Expect(_ => _.TrackedGraphForInternalId(internalIdOfCustomer)).Return(customer);

            var bytes = Subject.Serialize(root, mockSession);
            actual = Subject.Deserialize(bytes, mockSession);
        }

        [Then]
        public void it_should_deserialise_the_root_aggregate()
        {
            actual.ShouldBeOfType<Order>();
        }

        [Then]
        public void it_should_use_the_tracked_customer()
        {
            actual.ForCustomer.ShouldBeTheSameAs(customer);
        }
    }
}