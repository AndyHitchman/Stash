namespace Stash.Specifications.for_stashed_set
{
    using System;
    using BackingStore;
    using Queries;
    using Rhino.Mocks;
    using Support;

    public class when_getting_a_stashed_set : AutoMockedSpecification<StashedSet<DummyPersistentObject>>
    {
        private StashedSet<DummyPersistentObject> actual;
        private IQuery mockQuery;

        protected override void Given()
        {
//            Dependency<IBackingStore>().Stub()
            mockQuery = MockRepository.GenerateStub<IQuery>();
            actual = Subject.Where(mockQuery);
        }

        protected override void When()
        {
            actual.GetEnumerator().MoveNext();
        }

        [Then]
        public void it_should_do_stuff()
        {
        }
    }
}