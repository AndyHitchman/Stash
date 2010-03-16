#region License

// Copyright 2009 Andrew Hitchman
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
    using Rhino.Mocks;
    using Support;

    public class when_executing_intersect_two_queries : Specification
    {
        private IEnumerable<Guid> actual;
        private Guid commonGuid;
        private IBerkeleyQuery lhs;
        private IEnumerable<Guid> lhsSet;
        private IBerkeleyQuery rhs;
        private IEnumerable<Guid> rhsSet;
        private IntersectOperator sut;

        protected override void Given()
        {
            lhs = MockRepository.GenerateStub<IBerkeleyQuery>();
            rhs = MockRepository.GenerateStub<IBerkeleyQuery>();

            commonGuid = Guid.NewGuid();
            lhsSet = new[] {commonGuid, Guid.NewGuid()};
            rhsSet = new[] {Guid.NewGuid(), commonGuid};

            //A bit weak. We're not sure which side will execute first: The second to execute 
            //will call ExecuteInsideIntersect. Cheat by ignoring this and having the pair of methods
            //return the same result.
            lhs.Stub(_ => _.Execute(null, null)).IgnoreArguments().Return(lhsSet);
            lhs.Stub(_ => _.ExecuteInsideIntersect(null, null, null)).IgnoreArguments().Return(lhsSet);
            rhs.Stub(_ => _.Execute(null, null)).IgnoreArguments().Return(rhsSet);
            rhs.Stub(_ => _.ExecuteInsideIntersect(null, null, null)).IgnoreArguments().Return(rhsSet);

            sut = new IntersectOperator(new[] {lhs, rhs});
        }

        protected override void When()
        {
            actual = sut.Execute(null, null);
        }

        [Then]
        public void it_should_produce_only_elements_present_in_both_sides()
        {
            actual.ShouldHaveCount(1);
        }

        [Then]
        public void it_should_produce_the_intersect_of_the_two_sides()
        {
            actual.ShouldContain(commonGuid);
        }
    }
}