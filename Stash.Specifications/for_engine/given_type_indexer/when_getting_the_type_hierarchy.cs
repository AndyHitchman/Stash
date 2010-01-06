namespace Stash.Specifications.for_engine.given_type_indexer
{
    using NUnit.Framework;

    [TestFixture]
    public class when_getting_the_type_hierarchy
    {
        [Test]
        public void it_should_return_one_type_for_something_that_has_object_as_its_base_class()
        {
            var sut = new OpenedUpTypeIndexer();
            var actual = sut.OpenedGetTypeHierarchyFor(new ClassWithNoAncestors());
            actual.ShouldHaveCount(1);
        }

        [Test]
        public void it_should_return_only_the_object_type_for_something_that_has_object_as_its_base_class()
        {
            var sut = new OpenedUpTypeIndexer();
            var actual = sut.OpenedGetTypeHierarchyFor(new ClassWithNoAncestors());
            actual.ShouldContain(projection => projection.Key == typeof(ClassWithNoAncestors));
        }

        [Test]
        public void it_should_return_the_base_class_for_a_class_with_one_ancestor()
        {
            var sut = new OpenedUpTypeIndexer();
            var actual = sut.OpenedGetTypeHierarchyFor(new ClassWithOneAncestors());
            actual.ShouldContain(projection => projection.Key == typeof(ClassWithOneAncestors));
        }

        [Test]
        public void it_should_return_the_base_classes_for_a_class_with_many_ancestor()
        {
            var sut = new OpenedUpTypeIndexer();
            var actual = sut.OpenedGetTypeHierarchyFor(new ClassWithTwoAncestors());
            actual.ShouldContain(projection => projection.Key == typeof(ClassWithOneAncestors));
            actual.ShouldContain(projection => projection.Key == typeof(ClassWithTwoAncestors));
            actual.ShouldContain(projection => projection.Key == typeof(ClassWithNoAncestors));
        }

        [Test]
        public void it_should_return_three_types_for_a_class_with_two_ancestor()
        {
            var sut = new OpenedUpTypeIndexer();
            var actual = sut.OpenedGetTypeHierarchyFor(new ClassWithTwoAncestors());
            actual.ShouldHaveCount(3);
        }

        [Test]
        public void it_should_return_two_types_for_a_class_with_one_ancestor()
        {
            var sut = new OpenedUpTypeIndexer();
            var actual = sut.OpenedGetTypeHierarchyFor(new ClassWithOneAncestors());
            actual.ShouldHaveCount(2);
        }
    }
}