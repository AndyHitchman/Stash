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
    using System.Linq;
    using BackingStore.BDB.BerkeleyQueries;
    using Rhino.Mocks;
    using Support;

    public class when_executing_union : Specification
    {
        private IEnumerable<Guid> actual;
        private IBerkeleyQuery lhs;
        private IEnumerable<Guid> lhsSet;
        private IBerkeleyQuery rhs;
        private IEnumerable<Guid> rhsSet;
        private UnionOperator sut;

        protected override void Given()
        {
            lhs = MockRepository.GenerateStub<IBerkeleyQuery>();
            rhs = MockRepository.GenerateStub<IBerkeleyQuery>();

            var commonGuid = Guid.NewGuid();
            lhsSet = new[] {commonGuid, Guid.NewGuid()};
            rhsSet = new[] {Guid.NewGuid(), commonGuid};

            lhs.Stub(_ => _.Execute(null)).IgnoreArguments().Return(lhsSet);
            rhs.Stub(_ => _.Execute(null)).IgnoreArguments().Return(rhsSet);

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