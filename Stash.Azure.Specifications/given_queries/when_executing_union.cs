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
    using System.Linq;
    using Stash.Azure.AzureQueries;
    using Stash.Azure.Specifications.Support;
    using NSubstitute;
    using Stash.Engine;

    public class when_executing_union : Specification
    {
        private IEnumerable<InternalId> actual;
        private IAzureQuery lhs;
        private InternalId[] lhsSet;
        private IAzureQuery rhs;
        private InternalId[] rhsSet;
        private UnionOperator sut;

        protected override void Given()
        {
            lhs = Substitute.For<IAzureQuery>();
            rhs = Substitute.For<IAzureQuery>();

            var commonGuid = new InternalId(Guid.NewGuid());
            lhsSet = new[] {commonGuid, new InternalId(Guid.NewGuid())};
            rhsSet = new[] {new InternalId(Guid.NewGuid()), commonGuid};

            lhs.Execute(null).ReturnsForAnyArgs(lhsSet);
            rhs.Execute(null).ReturnsForAnyArgs(rhsSet);

            sut = new UnionOperator(new[] {lhs, rhs});
        }

        protected override void When()
        {
            actual = sut.Execute(null);
        }

        [Then]
        public void it_should_produce_the_union_of_the_two_sides()
        {
            actual.ShouldContain(lhsSet.First());
            actual.ShouldContain(lhsSet.Skip(1).First());
            actual.ShouldContain(rhsSet.First());
            actual.ShouldContain(rhsSet.Skip(1).First());
        }

        [Then]
        public void it_should_remove_duplicates()
        {
            actual.ShouldHaveCount(3);
        }
    }
}