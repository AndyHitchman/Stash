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

    public class when_deserializing_an_aggregate_with_circular_references : AutoMockedSpecification<AggregateBinarySerializer<GraphB>>
    {
        private GraphB graphB;
        private InternalId internalIdOfA;
        private ISerializationSession mockSession;
        private GraphB actualB;
        private IRegistry mockRegistry;
        private GraphA graphA;
        private IEnumerable<byte> bytes;
        private InternalId internalIdOfB;

        protected override void Given()
        {
            graphA = new GraphA {AString = "Bob"};
            graphB = new GraphB {AString = "Jane", GraphA = graphA};
            graphA.GraphB = graphB;

            internalIdOfA = new InternalId(new Guid("CFEBF6B6-FB86-49E0-A27D-9F63319AAA2D"));
            internalIdOfB = new InternalId(new Guid("7E3BF6C5-82FE-47A3-94D2-4373F7FCFDFA"));

            mockRegistry = MockRepository.GenerateMock<IRegistry>();
            mockRegistry.Stub(_ => _.IsManagingGraphTypeOrAncestor(typeof(GraphA))).Return(true);

            mockSession = MockRepository.GenerateMock<ISerializationSession>();
            mockSession.Stub(_ => _.InternalIdOfTrackedGraph(graphB)).Return(internalIdOfB);
            mockSession.Stub(_ => _.InternalIdOfTrackedGraph(graphA)).Return(internalIdOfA);

            Dependency<IRegisteredGraph<GraphA>>().Stub(_ => _.GraphType).Return(typeof(GraphA));
            Dependency<IRegisteredGraph<GraphA>>().Stub(_ => _.Registry).Return(mockRegistry);
            Dependency<IRegisteredGraph<GraphB>>().Stub(_ => _.GraphType).Return(typeof(GraphB));
            Dependency<IRegisteredGraph<GraphB>>().Stub(_ => _.Registry).Return(mockRegistry);

            bytes = Subject.Serialize(graphB, mockSession);
        }

        protected override void When()
        {
            mockSession.Expect(_ => _.TrackedGraphForInternalId(internalIdOfA)).Return(graphA);

            actualB = Subject.Deserialize(bytes, mockSession);
        }

        [Then]
        public void it_should_deserialise_the_root_aggregate()
        {
            actualB.ShouldBeOfType<GraphB>();
        }

        [Then]
        public void it_should_use_the_tracked_root_aggregate()
        {
            actualB.GraphA.ShouldBeTheSameAs(graphA);
        }

        [Then]
        public void it_should_determine_whether_the_referenced_aggregate_is_already_tracked()
        {
            mockSession.VerifyAllExpectations();
        }
    }
}