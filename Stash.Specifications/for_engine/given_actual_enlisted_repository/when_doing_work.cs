namespace Stash.Specifications.for_engine.given_actual_enlisted_repository
{
    using Engine;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_doing_work
    {
        [Test]
        public void it_should_delegate_all_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.All<DummyPersistentObject>();

            mockUnenlistedRepository.AssertWasCalled(repository => repository.All<DummyPersistentObject>(mockSession));
        }

        [Test]
        public void it_should_delegate_delete_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.Delete(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Delete(mockSession, graph));
        }

        [Test]
        public void it_should_delegate_get_tracker_for_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.GetTrackerFor(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.GetTrackerFor(mockSession, graph));
        }

        [Test]
        public void it_should_delegate_index_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var mockIndexer = MockRepository.GenerateMock<Index<DummyPersistentObject>>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.Index(mockIndexer);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Index(mockSession, mockIndexer));
        }

        [Test]
        public void it_should_delegate_index_many_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var mockIndexer = MockRepository.GenerateMock<Index<DummyPersistentObject>>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);
            var expected = new[] {mockIndexer};
            
            sut.Index(expected);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Index(mockSession, expected));
        }

        [Test]
        public void it_should_delegate_map_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var mockMapper = MockRepository.GenerateMock<Map<DummyPersistentObject,object,object>>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.Map<DummyPersistentObject,object,object>(mockMapper);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Map(mockSession, mockMapper));
        }

        [Test]
        public void it_should_delegate_persist_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.Persist(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Persist(mockSession, graph));
        }

        [Test]
        public void it_should_delegate_reconnect_tracker_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var mockTracker = MockRepository.GenerateMock<Tracker>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.ReconnectTracker(mockTracker);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.ReconnectTracker(mockSession, mockTracker));
        }

        [Test]
        public void it_should_delegate_reduce_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var mockReducer = MockRepository.GenerateMock<Reduction<object, object>>();
            var sut = new ActualEnlistedRepository(mockSession, mockUnenlistedRepository);
            var expected = new object();
            
            sut.Reduce<object,object>(expected, mockReducer);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Reduce<object, object>(mockSession, expected, mockReducer));
        }
    }
}