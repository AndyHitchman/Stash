namespace Stash.Specifications.for_stashed_set
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using Queries;
    using Rhino.Mocks;
    using Support;

    public class when_getting_a_stashed_set : AutoMockedSpecification<StashedSet<DummyPersistentObject>>
    {
        private IEnumerator<DummyPersistentObject> actual;
        private IQuery mockQuery;
        private IStoredGraph mockStoredGraph;
        private IBackingStore mockBackingStore;
        private IRegisteredGraph<DummyPersistentObject> mockRegisteredGraph;
        private ITrack<DummyPersistentObject> mockTrack;

        protected override void Given()
        {
            mockQuery = MockRepository.GenerateStub<IQuery>();

            mockBackingStore = MockRepository.GenerateStub<IBackingStore>();
            Dependency<IRegistry>().Stub(_ => _.BackingStore).Return(mockBackingStore);

            mockRegisteredGraph = MockRepository.GenerateStub<IRegisteredGraph<DummyPersistentObject>>();
            Dependency<IRegistry>().Stub(_ => _.GetRegistrationFor(typeof(DummyPersistentObject))).Return(mockRegisteredGraph);

            mockStoredGraph = MockRepository.GenerateStub<IStoredGraph>();
            mockBackingStore.Stub(_ => _.Get(null)).IgnoreArguments().Return(new[] {mockStoredGraph});

            mockTrack = MockRepository.GenerateStub<ITrack<DummyPersistentObject>>();
            Dependency<IInternalSession>().Stub(_ => _.Track<DummyPersistentObject>(null, null)).IgnoreArguments().Return(mockTrack);

        }

        protected override void When()
        {
            actual = Subject.Where(mockQuery).GetEnumerator();
            actual.MoveNext();
        }

        [Then]
        public void it_should_do_stuff()
        {
        }
    }
}