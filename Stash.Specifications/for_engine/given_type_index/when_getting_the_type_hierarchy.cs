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

namespace Stash.Specifications.for_engine.given_type_index
{
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_getting_the_type_hierarchy
    {
        [Test]
        public void it_should_return_one_type_for_something_that_has_object_as_its_base_class()
        {
            var sut = new OpenedUpStashTypeHierarchy();
            var actual = sut.OpenedGetTypeHierarchyFor(typeof(ClassWithNoAncestors));
            actual.ShouldHaveCount(1);
        }

        [Test]
        public void it_should_return_only_the_object_type_for_something_that_has_object_as_its_base_class()
        {
            var sut = new OpenedUpStashTypeHierarchy();
            var actual = sut.OpenedGetTypeHierarchyFor(typeof(ClassWithNoAncestors));
            actual.ShouldContain(_ => _ == typeof(ClassWithNoAncestors));
        }

        [Test]
        public void it_should_return_the_base_class_for_a_class_with_one_ancestor()
        {
            var sut = new OpenedUpStashTypeHierarchy();
            var actual = sut.OpenedGetTypeHierarchyFor(typeof(ClassWithOneAncestors));
            actual.ShouldContain(_ => _ == typeof(ClassWithOneAncestors));
        }

        [Test]
        public void it_should_return_the_base_classes_for_a_class_with_many_ancestor()
        {
            var sut = new OpenedUpStashTypeHierarchy();
            var actual = sut.OpenedGetTypeHierarchyFor(typeof(ClassWithTwoAncestors));
            actual.ShouldContain(_ => _ == typeof(ClassWithOneAncestors));
            actual.ShouldContain(_ => _ == typeof(ClassWithTwoAncestors));
            actual.ShouldContain(_ => _ == typeof(ClassWithNoAncestors));
        }

        [Test]
        public void it_should_return_three_types_for_a_class_with_two_ancestor()
        {
            var sut = new OpenedUpStashTypeHierarchy();
            var actual = sut.OpenedGetTypeHierarchyFor(typeof(ClassWithTwoAncestors));
            actual.ShouldHaveCount(3);
        }

        [Test]
        public void it_should_return_two_types_for_a_class_with_one_ancestor()
        {
            var sut = new OpenedUpStashTypeHierarchy();
            var actual = sut.OpenedGetTypeHierarchyFor(typeof(ClassWithOneAncestors));
            actual.ShouldHaveCount(2);
        }
    }
}