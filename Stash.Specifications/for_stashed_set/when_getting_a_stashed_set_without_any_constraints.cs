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

namespace Stash.Specifications.for_stashed_set
{
    using System;
    using Support;

    public class when_getting_a_stashed_set_without_any_constraints : AutoMockedSpecification<StashedSet<DummyPersistentObject>>
    {
        protected override void Given() {}

        protected override void When() {}

        [Then]
        public void it_should_complain()
        {
            typeof(InvalidOperationException).ShouldBeThrownBy(() => Subject.GetEnumerator());
        }
    }
}