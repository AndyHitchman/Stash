namespace Stash.Specifications.for_stashed_set
{
    using System;
    using Queries;
    using Support;

    public class when_getting_a_stashed_set : Specification
    {
        private StashedSet<DummyPersistentObject> sut;

        protected override void Given()
        {
            sut = new StashedSet<DummyPersistentObject>(null, null);
        }

        protected override void When()
        {
            sut.Where(Index<DummyIndex>.AllOf(new[] {1, 2, 3}));
            sut.Where(new DummyIndex().AllOf(new[] {1, 2, 3}));

            sut.Where(Index.IntersectionOf(new DummyIndex().AllOf(new[] {1, 2, 3}), new DummyIndex().AllOf(new[] {2, 3})));
            sut.Where(Index<DummyIndex>.AllOf(new[] {1, 2, 3}).And(Index<DummyIndex>.AllOf(new[] {2, 3})));
        }

        [Then]
        public void it_should_do_stuff()
        {
            
        }
    }
}