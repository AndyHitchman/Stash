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
    using System.Collections.Generic;
    using Configuration;
    using Engine;
    using Engine.Serializers.Binary;
    using Rhino.Mocks;
    using Support;

    public class when_serializing_an_aggregate_referencing_another_aggregate : AutoMockedSpecification<AggregateBinarySerializer<Order>>
    {
        private Order root;
        private Guid internalIdOfCustomer;
        private IInternalSession mockTrackedSession;
        private IEnumerable<byte> actual;
        private IRegistry mockRegistry;

        protected override void Given()
        {
            mockRegistry = MockRepository.GenerateMock<IRegistry>();
            Dependency<IRegisteredGraph<Order>>().Stub(_ => _.GraphType).Return(typeof(Order));
            Dependency<IRegisteredGraph<Order>>().Stub(_ => _.Registry).Return(mockRegistry);

            root = new Order {ForCustomer = new Customer {Name = "Bob"}};

            internalIdOfCustomer = Guid.NewGuid();

            mockTrackedSession = MockRepository.GenerateMock<IInternalSession>();
        }

        protected override void When()
        {
            mockRegistry.Expect(_ => _.IsManagingGraphTypeOrAncestor(typeof(Customer))).Return(true);
            mockTrackedSession.Expect(_ => _.InternalIdOfTrackedGraph(root.ForCustomer)).Return(internalIdOfCustomer);

            actual = Subject.Serialize(root, mockTrackedSession);
        }

        [Then]
        public void it_should_serialise_the_root_aggregate_not_a_reference()
        {
            mockTrackedSession.AssertWasNotCalled(_ => _.InternalIdOfTrackedGraph(root));
        }

        [Then]
        public void it_should_serialise_a_reference_to_the_customer_object()
        {
            mockTrackedSession.AssertWasCalled(_ => _.InternalIdOfTrackedGraph(root.ForCustomer));
        }
    }
}