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

namespace Stash.Specifications.for_backingstore_bsb.given_queries
{
    using System;
    using System.Collections.Generic;
    using BackingStore.BDB.BerkeleyQueries;
    using Engine;
    using Rhino.Mocks;
    using Support;

    public class when_executing_union_inside_intersect : Specification
    {
        private IEnumerable<InternalId> actual;
        private IBerkeleyQuery lhs;
        private IBerkeleyQuery rhs;
        private UnionOperator sut;
        private InternalId intersectingGuid1;
        private InternalId[] joinConstraint;
        private InternalId intersectingGuid2;

        protected override void Given()
        {
            lhs = MockRepository.GenerateStub<IBerkeleyQuery>();
            rhs = MockRepository.GenerateStub<IBerkeleyQuery>();

            intersectingGuid1 = new InternalId(Guid.NewGuid());
            intersectingGuid2 = new InternalId(Guid.NewGuid());

            lhs.Stub(_ => _.ExecuteInsideIntersect(null, null)).IgnoreArguments().Return(new[] {intersectingGuid1});
            rhs.Stub(_ => _.ExecuteInsideIntersect(null, null)).IgnoreArguments().Return(new[] {intersectingGuid2});

            sut = new UnionOperator(new[] {lhs, rhs});

            joinConstraint = new[]
                {
                    intersectingGuid1, new InternalId(Guid.NewGuid()), intersectingGuid2
                };
        }

        protected override void When()
        {
            actual = sut.ExecuteInsideIntersect(null, joinConstraint);
        }

        [Then]
        public void it_should_produce_the_intersection_of_the_union_of_the_two_sides()
        {
            actual.ShouldContain(intersectingGuid1);
            actual.ShouldContain(intersectingGuid2);
        }

        [Then]
        public void it_should_leave_only_the_intersecting()
        {
            actual.ShouldHaveCount(2);
        }
    }
}