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

namespace Stash.Specifications.for_engine.for_serializers.given_aggregate_binary_serializer
{
    using System;
    using Configuration;
    using Engine;
    using Engine.Serializers.Binary;
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
            customer = new Customer {Name = "Bob"};
            root = new Order {ForCustomer = customer};

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