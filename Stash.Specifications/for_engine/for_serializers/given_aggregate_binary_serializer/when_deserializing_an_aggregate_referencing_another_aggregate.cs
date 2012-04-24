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
    using Rhino.Mocks;
    using Serializers.Binary;
    using Support;

    public class when_deserializing_an_aggregate_referencing_another_aggregate : AutoMockedSpecification<AggregateBinarySerializer<GraphB>>
    {
        private GraphB root;
        private InternalId internalIdOfCustomer;
        private ISerializationSession mockSession;
        private GraphB actual;
        private IRegistry mockRegistry;
        private GraphA graphA;
        private IEnumerable<byte> bytes;
        private InternalId internalIdOfOrder;

        protected override void Given()
        {
            graphA = new GraphA {AString = "Bob"};
            root = new GraphB {GraphA = graphA};

            internalIdOfCustomer = new InternalId(Guid.NewGuid());
            internalIdOfOrder = new InternalId(Guid.NewGuid());

            mockRegistry = MockRepository.GenerateMock<IRegistry>();
            mockRegistry.Stub(_ => _.IsManagingGraphTypeOrAncestor(typeof(GraphA))).Return(true);

            mockSession = MockRepository.GenerateMock<ISerializationSession>();
            mockSession.Stub(_ => _.InternalIdOfTrackedGraph(root)).Return(internalIdOfOrder);
            mockSession.Stub(_ => _.InternalIdOfTrackedGraph(root.GraphA)).Return(internalIdOfCustomer);

            Dependency<IRegisteredGraph<GraphB>>().Stub(_ => _.GraphType).Return(typeof(GraphB));
            Dependency<IRegisteredGraph<GraphB>>().Stub(_ => _.Registry).Return(mockRegistry);

            bytes = Subject.Serialize(root, mockSession);
        }

        protected override void When()
        {
            mockSession.Expect(_ => _.TrackedGraphForInternalId(internalIdOfCustomer)).Return(graphA);

            actual = Subject.Deserialize(bytes, mockSession);
        }

        [Then]
        public void it_should_deserialise_the_root_aggregate()
        {
            actual.ShouldBeOfType<GraphB>();
        }

        [Then]
        public void it_should_use_the_tracked_customer()
        {
//            mockSession.VerifyAllExpectations();
            actual.GraphA.ShouldBeTheSameAs(graphA);
        }
    }
}