namespace Stash.Specifications.for_engine.for_persistence_events.given_track
{
    using System.Linq;
    using BackingStore;
    using Rhino.Mocks;
    using Support;

    public class when_initialising_track_with_empty_serialized_graph : AutoMockedSpecification<StandInTrack<DummyPersistentObject>>
    {
        protected override void Given()
        {
            var serializedGraph = Enumerable.Empty<byte>();
            Dependency<IStoredGraph>().Expect(_ => _.SerialisedGraph).Return(serializedGraph);
        }

        protected override void When()
        {
            var instantiate = Subject;
        }

        [Then]
        public void it_should_calculate_a_hash_code_based_on_the_empty_serialized_graph()
        {
            Subject.OriginalHash.ShouldNotBeEmpty();
        }
    }
}