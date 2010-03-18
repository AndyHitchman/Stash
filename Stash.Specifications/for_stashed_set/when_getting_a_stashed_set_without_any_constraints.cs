namespace Stash.Specifications.for_stashed_set
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Queries;
    using Support;

    public class when_getting_a_stashed_set_without_any_constraints : AutoMockedSpecification<StashedSet<DummyPersistentObject>>
    {
        private IEnumerator<DummyPersistentObject> actual;

        protected override void Given()
        {
        }

        protected override void When()
        {
            actual = Subject.GetEnumerator();
        }

        [Then]
        public void it_should_complain()
        {
            typeof(InvalidOperationException).ShouldBeThrownBy(() => actual.MoveNext());
        }
    }
}