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
    using Rhino.Mocks;
    using Support;

    public class when_executing_intersect_with_one_query : Specification
    {
        private IEnumerable<Guid> actual;
        private IBerkeleyQuery lhs;
        private IEnumerable<Guid> lhsSet;
        private IntersectOperator sut;

        protected override void Given()
        {
            lhs = MockRepository.GenerateStub<IBerkeleyQuery>();

            lhsSet = new[] {Guid.NewGuid(), Guid.NewGuid()};

            lhs.Stub(_ => _.Execute(null)).IgnoreArguments().Return(lhsSet);
            lhs.Stub(_ => _.ExecuteInsideIntersect(null, null)).IgnoreArguments().Return(lhsSet);

            sut = new IntersectOperator(new[] {lhs});
        }

        protected override void When()
        {
            actual = sut.Execute(null);
        }

        [Then]
        public void it_should_produce_all_of_the_elements_present_in_the_only_query()
        {
            actual.ShouldHaveCount(2);
        }
    }
}