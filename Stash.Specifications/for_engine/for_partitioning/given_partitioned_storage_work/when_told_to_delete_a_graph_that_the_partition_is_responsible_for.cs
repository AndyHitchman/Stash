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

namespace Stash.Specifications.for_engine.for_partitioning.given_partitioned_storage_work
{
    using System;
    using BackingStore;
    using Configuration;
    using Engine;
    using Engine.Partitioning;
    using Rhino.Mocks;
    using Support;

    public class when_told_to_delete_a_graph_that_the_partition_is_responsible_for : AutoMockedSpecification<PartitionedStorageWork>
    {
        private InternalId internalId;

        protected override void WithContext()
        {
            internalId = new InternalId(Guid.NewGuid());
        }

        protected override void Given()
        {
            Dependency<IPartition>().Expect(_ => _.IsResponsibleForGraph(Arg<InternalId>.Is.Anything)).Return(true);
        }

        protected override void When()
        {
            Subject.DeleteGraph(internalId, null);
        }

        [Then]
        public void it_should_ask_the_partition_whether_it_manages_the_graph()
        {
            Dependency<IPartition>().VerifyAllExpectations();
        }

        [Then]
        public void it_should_delete_the_graph()
        {
            Dependency<IStorageWork>().AssertWasCalled(_ => _.DeleteGraph(Arg.Is(internalId), Arg<IRegisteredGraph>.Is.Anything));
        }
    }
}