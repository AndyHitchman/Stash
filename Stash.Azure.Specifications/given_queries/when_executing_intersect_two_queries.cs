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

namespace Stash.Azure.Specifications.given_queries
{
    using System;
    using System.Collections.Generic;
    using NSubstitute;
    using Stash.Azure.AzureQueries;
    using Stash.Azure.Specifications.Support;
    using Stash.Engine;

    public class when_executing_intersect_two_queries : Specification
    {
        private IEnumerable<InternalId> actual;
        private InternalId commonGuid;
        private IAzureQuery lhs;
        private InternalId[] lhsSet;
        private IAzureQuery rhs;
        private InternalId[] rhsSet;
        private IntersectOperator sut;

        protected override void Given()
        {
            lhs = Substitute.For<IAzureQuery>();
            rhs = Substitute.For<IAzureQuery>();

            commonGuid = new InternalId(Guid.NewGuid());
            lhsSet = new[] {commonGuid, new InternalId(Guid.NewGuid())};
            rhsSet = new[] {new InternalId(Guid.NewGuid()), commonGuid};

            //A bit weak. We're not sure which side will execute first: The second to execute 
            //will call ExecuteInsideIntersect. Cheat by ignoring this and having the pair of methods
            //return the same result.
            lhs.Execute(null).ReturnsForAnyArgs(lhsSet);
            lhs.ExecuteInsideIntersect(null, null).ReturnsForAnyArgs(lhsSet);
            rhs.Execute(null).ReturnsForAnyArgs(rhsSet);
            rhs.ExecuteInsideIntersect(null, null).ReturnsForAnyArgs(rhsSet);

            sut = new IntersectOperator(new[] {lhs, rhs});
        }

        protected override void When()
        {
            actual = sut.Execute(null);
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